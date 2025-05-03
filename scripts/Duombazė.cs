using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class Duombazė : Node
{
	// Prisijungimo duomenų saugojimas
	private string vartotojoVardas;
	private string užšifruotasSlaptažodis;
	private byte[] druskosReikšmė;

	// Žaidėjo progreso duomenys
	private int žaidėjoLygis;
	private int patirtiesTaškai;
	private int surinktaTaškų;
	private string paskutinėPozicija;

	// Called when the node enters the scene tree for the first time.
	public class Hero
	{
		public Hero(string Name)
		{
			this.Name = Name;
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Cordinates { get; set; }
		public int Hp { get; set; }
		public int Mana { get; set; }
		public int Money { get; set; }
		public int Defence { get; set; }
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

	// Funkcija prisijungimo duomenims saugoti
	public void SaugotiPrisijungimoDuomenis(string vardas, string slaptažodis)
	{
		vartotojoVardas = vardas;

		// Slaptažodžio užšifravimas
		druskosReikšmė = GeneruotiDruskosReikšmę();
		užšifruotasSlaptažodis = UžšifruotiSlaptažodį(slaptažodis, druskosReikšmė);

		// Išsaugoti duomenis į failą
		IšsaugotiDuomenis();
	}

	private byte[] GeneruotiDruskosReikšmę()
	{
		byte[] druska = new byte[16];
		using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
		{
			rng.GetBytes(druska);
		}
		return druska;
	}

	private string UžšifruotiSlaptažodį(string slaptažodis, byte[] druska)
	{
		using (var pbkdf2 = new Rfc2898DeriveBytes(slaptažodis, druska, 10000))
		{
			byte[] hash = pbkdf2.GetBytes(20);
			return Convert.ToBase64String(hash);
		}
	}

	// Žaidėjo progreso saugojimo funkcijos
	public void SaugotiŽaidėjoProgresą(int lygis, int patirtis, int taškai)
	{
		žaidėjoLygis = lygis;
		patirtiesTaškai = patirtis;
		surinktaTaškų = taškai;

		IšsaugotiDuomenis();
	}

	// Žaidėjo pozicijos saugojimo funkcija
	public void SaugotiŽaidėjoPoziciją(string koordinatės)
	{
		paskutinėPozicija = koordinatės;
		herojus.Cordinates = koordinatės;

		IšsaugotiDuomenis();
	}

	// Duomenų išsaugojimas į failą
	private void IšsaugotiDuomenis()
	{
		string duomenųFailas = "user://zaidejas.save";

		using (var file = new FileInfo(duomenųFailas).CreateText())
		{
			file.WriteLine($"VartotojoVardas:{vartotojoVardas}");
			file.WriteLine($"UžšifruotasSlaptažodis:{užšifruotasSlaptažodis}");
			file.WriteLine($"DruskosReikšmė:{Convert.ToBase64String(druskosReikšmė)}");
			file.WriteLine($"Lygis:{žaidėjoLygis}");
			file.WriteLine($"Patirtis:{patirtiesTaškai}");
			file.WriteLine($"Taškai:{surinktaTaškų}");
			file.WriteLine($"Pozicija:{paskutinėPozicija}");
		}
	}

	// Duomenų užkrovimas
	public bool UžkrautiDuomenis()
	{
		string duomenųFailas = "user://zaidejas.save";

		if (!File.Exists(duomenųFailas))
			return false;

		try
		{
			string[] eilutės = File.ReadAllLines(duomenųFailas);
			foreach (string eilutė in eilutės)
			{
				string[] dalys = eilutė.Split(':');
				if (dalys.Length != 2)
					continue;

				switch (dalys[0])
				{
					case "VartotojoVardas":
						vartotojoVardas = dalys[1];
						break;
					case "UžšifruotasSlaptažodis":
						užšifruotasSlaptažodis = dalys[1];
						break;
					case "DruskosReikšmė":
						druskosReikšmė = Convert.FromBase64String(dalys[1]);
						break;
					case "Lygis":
						žaidėjoLygis = int.Parse(dalys[1]);
						break;
					case "Patirtis":
						patirtiesTaškai = int.Parse(dalys[1]);
						break;
					case "Taškai":
						surinktaTaškų = int.Parse(dalys[1]);
						break;
					case "Pozicija":
						paskutinėPozicija = dalys[1];
						herojus.Cordinates = dalys[1];
						break;
				}
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	// Slaptažodžio tikrinimas
	public bool TikrintiSlaptažodį(string įvestasSlaptažodis)
	{
		string užšifruotasĮvestasSlaptažodis = UžšifruotiSlaptažodį(įvestasSlaptažodis, druskosReikšmė);
		return užšifruotasĮvestasSlaptažodis == užšifruotasSlaptažodis;
	}
}
