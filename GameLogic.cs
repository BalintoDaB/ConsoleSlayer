using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public class GameLogic
    {
        public Game Game { get; set; }
        private readonly Random RNG = new Random();

        public GameLogic(Game game) 
        { 
            Game = game;
        }

        public void PlayerAttackLogic()
        {
            if (Game.Player.Ammo != 0)
            {
                Game.Player.Shoot();
                List<Demon> demons = GetDemonsWithinDistance(Game.Player.Position, Game.Player.SightRange);
                foreach (var item in demons)
                {
                    float d = Position.Distance(Game.Player.Position, item.Position);
                    float dmg = (2 * RNG.Next(35, 105)) / (1 + d);
                    item.TakeDamage((int)dmg);
                    Game.PlaySoundEffect(SoundEffectType.Shotgun);
                    if (!item.Alive)
                    {
                        if (item.Type == DemonType.Zombieman) Game.Player.AddCombatPoints(1);
                        if (item.Type == DemonType.Imp) Game.Player.AddCombatPoints(3);
                        if (item.Type == DemonType.Mancubus) Game.Player.AddCombatPoints(10);
                    }
                }
            }
        }

        public void PlayerBFGAttackLogic()
        {
            if (Game.Player.BFGCells != 0)
            {
                Game.Player.ShootBFG();
                List<Demon> demons = GetDemonsWithinDistance(Game.Player.Position, Game.Player.SightRange);
                foreach (var item in demons)
                {
                    item.TakeDamage(RNG.Next(100, 800));
                    Game.PlaySoundEffect(SoundEffectType.BFG);
                    if (!item.Alive)
                    {
                        Game.Player.AddCombatPoints(1);
                    }
                }
            }
        }
        private void DemonAttackLogic(Demon demon, long deltaT)
        {
            float d = Position.Distance(Game.Player.Position, demon.Position);
            float rng = 0;
            if (demon.Type == DemonType.Zombieman) rng = RNG.Next(3, 15);
            if (demon.Type == DemonType.Imp) rng = RNG.Next(3, 24);
            if (demon.Type == DemonType.Mancubus) rng = RNG.Next(8, 64);
            float dmg = (2 * rng) / (1 + d) * (deltaT / 1000f);
            Game.Player.TakeDamage((int)dmg);
            Game.PlaySoundEffect(SoundEffectType.Pain);
        }

        private void CleanUpGameItems()
        {
            List<GameItem> toDelete = new List<GameItem>();
            foreach (var item in Game.Items)
            {
                if (!item.Available)
                {
                    toDelete.Add(item);
                }
            }
            foreach (var item in toDelete)
            {
                Game.Items.Remove(item);
            }
        }
        private void CleanUpDemons()
        {
            List<Demon> toDelete = new List<Demon>();
            foreach (var demon in Game.Demons)
            {
                if (!demon.Alive)
                {
                    toDelete.Add(demon);
                }
            }
            foreach (var demon in toDelete)
            {
                Game.Demons.Remove(demon);
            }
        }

        private List<GameItem> GetGameItemsWithinDistance(Position position, double distance)
        {
            List<GameItem> items = new List<GameItem>();
            foreach (var item in Game.Items)
            {
                if (Position.Distance(position, item.Position) <= distance) items.Add(item);
            }
            return items;
        }

        private List<Demon> GetDemonsWithinDistance(Position position, double distance)
        {
            List<Demon> items = new List<Demon>();
            foreach (var item in Game.Demons)
            {
                if (Position.Distance(position, item.Position) <= distance) items.Add(item);
            }
            return items;
        }

        private double GetTotalFillingRatio(Position position)
        {
            List<GameItem> items = GetGameItemsWithinDistance(position, 0);
            List<Demon> demons = GetDemonsWithinDistance(position, 0);
            return ((items.Sum(x => x.FillingRatio)) + (demons.Sum(x => x.FillingRatio)));
        }

        public void Move(Player player, Position position)
        {
            if ((GetTotalFillingRatio(position) + player.FillingRatio) > 1.0) return;
            player.Position = position;
        }

        public void Move(Demon demon, Position position, long deltaT)
        {
            double pMove = (demon.Speed / 100.0) * (deltaT / 1000.0);
            if (RNG.NextDouble() >= pMove) return;

            if ((GetTotalFillingRatio(position) + demon.FillingRatio) > 1.0) return;
            demon.Position = position;
        }

        private void DemonMoveLogic(Demon demon, long deltaT)
        {
            int x = demon.Position.x + RNG.Next(-1, 2); // -1, 0, 1
            int y = demon.Position.y + RNG.Next(-1, 2);
            Position targetPosition = new Position(x, y);
            Move(demon, targetPosition, deltaT);
        }

        private void UpdateDemons(long deltaT)
        {
            foreach (var demon in Game.Demons)
            {
                demon.UpdateState(Game.Player);
                if (demon.State == DemonStateType.Move) DemonMoveLogic(demon, deltaT);
                if (demon.State == DemonStateType.Attack) DemonAttackLogic(demon, deltaT);
                DemonIndirectInteractionLogic(demon);
            }
        }

        public void PlayerDirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(Game.Player.Position, 1);
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Door:
                        item.Interact();
                        Game.PlaySoundEffect(SoundEffectType.Door);
                        break;
                    case ItemType.LevelExit:
                        item.Interact();
                        Game.Exited = true;
                        break;
                }
                        
            }
        }

        public void PlayerIndirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(Game.Player.Position, 0);
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Ammo:
                        item.Interact();
                        Game.Player.PickUpAmmo(5);
                        Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                        break;
                    case ItemType.BFGCell:
                        item.Interact();
                        Game.Player.PickUpBFGCell(1);
                        Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                        break;
                    case ItemType.Medikit:
                        item.Interact();
                        Game.Player.PickUpHealth(25);
                        Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                        break;
                    case ItemType.ToxicWaste:
                        item.Interact();
                        Game.Player.TakeDamage(5);
                        Game.PlaySoundEffect(SoundEffectType.ItemPickup);
                        break;
                }

            }
        }

        private void DemonIndirectInteractionLogic(Demon demon)
        {
            List<GameItem> items = GetGameItemsWithinDistance(demon.Position, 0);
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case ItemType.ToxicWaste:
                        item.Interact();
                        demon.TakeDamage(5);
                        break;
                }

            }
        }
        public void UpdateGameState(long deltaT)
        {
            UpdateDemons(deltaT);
            CleanUpGameItems();
            CleanUpDemons();
        }
    }
}
