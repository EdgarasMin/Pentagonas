using Godot;
using System;
using System.Collections.Generic;

public partial class Duombazė : Node
{
    // Called when the node enters the scene tree for the first time.
    public class Hero {
        public Hero(string Name)
        {
            this.Name = Name;
        }
        int Id { get; set; }
        string Name { get; set; }
        string Cordinates { get; set; }
        int Hp { get; set; }
        int Mana { get; set; }
        int Money { get; set; }
        int Defence { get; set; }
    }
    public class Enemies
    {
        LinkedList<Enemie> listEnemies;
        void AddEnemie(LinkedList<Enemie> listEnemies, Enemie enemy) 
        { 
            listEnemies.AddLast(enemy);
        }
        void RemoveEnemie(LinkedList<Enemie> listEnemies, Enemie enemy)
        {
            listEnemies.Remove(enemy);
        }

    }
    public class Enemie
    {
        int Id { get; set; }
        string Name { get; set; }
        int Hp { get; set; }
        int Mana { get; set; }
        int Damage { get; set; }
        string AttacType { get; set; }
    }
    public class EnemyAlg
    { 
        public int Id { get; set; }
        string Alg { get; set; }
        void EnemyMove()
        {
            //nepabaigta
        }
    }
    Hero herojus = new Hero("Herojus"); 
    
    }
