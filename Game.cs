using System;
using System.Collections.Generic;
using System.Diagnostics;
using WMPLib;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public enum SoundEffectType { BFG, Door, ItemPickup, Pain, PlayerDeath, Shotgun }

    public class Game
    {
        public Player Player { get; set; }
        public bool Exited { get; set; } = false;
        public List<GameItem> Items { get; set; }
        public List<Demon> Demons { get; set; }

        public ConsoleRenderer Renderer { get; set; }
        public GameLogic GameLogic { get; set; }
        public Stopwatch StopwatchLogic { get; set; }
        public Stopwatch StopwatchRenderer { get; set; }

        public WindowsMediaPlayer MusicPlayer { get; set; }
        public WindowsMediaPlayer SfxPlayer { get; set; }

        public Game()
        {
            this.Player = new Player(0,0);
            this.Items = new List<GameItem>();
            this.Demons = new List<Demon>();
            Renderer = new ConsoleRenderer(this);
            GameLogic = new GameLogic(this);
            MusicPlayer = new WindowsMediaPlayer();
            SfxPlayer = new WindowsMediaPlayer();
        }

        

        private void UserAction()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);
                switch (pressed.Key)
                {
                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                    case ConsoleKey.UpArrow:
                        GameLogic.Move(Player, new Position(Player.Position.x, Player.Position.y-1));
                        break;
                    case ConsoleKey.LeftArrow:
                        GameLogic.Move(Player, new Position(Player.Position.x-1, Player.Position.y));
                        break;
                    case ConsoleKey.DownArrow:
                        GameLogic.Move(Player, new Position(Player.Position.x, Player.Position.y+1));
                        break;
                    case ConsoleKey.RightArrow:
                        GameLogic.Move(Player, new Position(Player.Position.x+1, Player.Position.y));
                        break;
                    case ConsoleKey.D:
                        GameLogic.PlayerDirectInteractionLogic();
                        break;
                    case ConsoleKey.A:
                        GameLogic.PlayerAttackLogic();
                        break;
                }
            }
            GameLogic.PlayerIndirectInteractionLogic();
        }

        public void PlaySoundEffect(SoundEffectType type)
        {
            switch (type)
            {
                case SoundEffectType.Door:
                    SfxPlayer.URL = "sound/door.mp3";
                    break;
                case SoundEffectType.ItemPickup:
                    SfxPlayer.URL = "sound/item_pickup.mp3";
                    break;
            }
            SfxPlayer.controls.play();
        }

        public void PlayMusic(string musicPath)
        {
            MusicPlayer.URL = musicPath;
            MusicPlayer.controls.play();
        }

        public void LoadMapFromPlainText(string fileLocation)
        {
            Items.Clear();
            Demons.Clear();

            using (StreamReader reader = new StreamReader(fileLocation))
            {
                int y = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    int x = 0;
                    foreach (var c in line)
                    {
                        switch (c)
                        {
                            case 'A': Items.Add(new GameItem(x, y, ItemType.Ammo)); break;
                            case 'B': Items.Add(new GameItem(x, y, ItemType.BFGCell)); break;
                            case 'D': Items.Add(new GameItem(x, y, ItemType.Door)); break;
                            case 'E': Items.Add(new GameItem(x, y, ItemType.LevelExit)); break;
                            case 'M': Items.Add(new GameItem(x, y, ItemType.Medikit)); break;
                            case 'T': Items.Add(new GameItem(x, y, ItemType.ToxicWaste)); break;
                            case 'W': Items.Add(new GameItem(x, y, ItemType.Wall)); break;

                            case 'z': Demons.Add(new Demon(x, y, DemonType.Zombieman)); break;
                            case 'i': Demons.Add(new Demon(x, y, DemonType.Imp)); break;
                            case 'm': Demons.Add(new Demon(x, y, DemonType.Mancubus)); break;

                            case 'p': Player.Position = new Position(x, y); break;

                            case '_': break; // üres csempe
                        }
                        x++;
                    }
                    y++;
                }
            }
        }

        public void Run()
        {
            StopwatchLogic = Stopwatch.StartNew();
            StopwatchRenderer = Stopwatch.StartNew();
            PlayMusic("sound/music.mp3");
            while (!Exited && Player.Alive)
            {
                if (StopwatchLogic.ElapsedMilliseconds > 500)
                {
                    GameLogic.UpdateGameState();
                    StopwatchLogic.Restart();
                }
                if (StopwatchRenderer.ElapsedMilliseconds > 25)
                {
                    Renderer.RenderGame();
                    StopwatchRenderer.Restart();
                }
                UserAction();
                Thread.Sleep(25);
            }
            if (!Player.Alive)
            {
                PlaySoundEffect(SoundEffectType.PlayerDeath);
            }
            if (Exited)
            {
                PlayMusic("sound/music_win.mp3");
            }
            Console.ReadLine();
        }
    }
}
