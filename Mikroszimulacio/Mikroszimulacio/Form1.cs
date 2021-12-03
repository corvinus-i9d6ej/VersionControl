using Mikroszimulacio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikroszimulacio
{
    public partial class Form1 : Form
    {
        List<Person> People = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<int> NumberOfMales = new List<int>();
        List<int> NumberOfFemales = new List<int>();

        Random rng = new Random(1234);

        public Form1()
        {
            InitializeComponent();
        }

        private void DisplayResults()
        {
            int counter = 0;
            for (int year = 2005; year < numericUpDown1.Value; year++)
            {
                richTextBox1.Text += 
                    string.Format("Szimulációs év: {0}\n\tFérfiak: {1}\n\tNők: {2}\n\n", year, NumberOfMales[counter], NumberOfFemales[counter]);
                counter++;
            }
        }

        private void Simulation()
        {
            People = GetPeople(textBoxPath.Text);
            BirthProbabilities = GetBirthProbabilities(@"C:\temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\temp\halál.csv");

            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                for (int i = 0; i < People.Count; i++)
                {
                    SimStep(year, People[i]);
                }

                int numberOfMales = (from x in People
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                NumberOfMales.Add(numberOfMales);

                int numberOfFemales = (from x in People
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();
                NumberOfFemales.Add(numberOfFemales);

                //Console.WriteLine(string.Format("Év: {0} Férfiak: {1} Nők: {2}", year, numberOfMales, numberOfFemales));
            }
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            byte age = (byte)(year - person.BirthYear);
            double deathProb = (from x in DeathProbabilities
                                where x.Gender == person.Gender && x.Age == age
                                select x.Probability).FirstOrDefault();
            if (rng.NextDouble() <= deathProb)
                person.IsAlive = false;
            if(person.IsAlive && person.Gender == Gender.Female)
            {
                double birthProb = (from x in BirthProbabilities
                                    where x.Age == age
                                    select x.Probability).FirstOrDefault();
                if (rng.NextDouble() <= birthProb)
                {
                    Person newBorn = new Person();
                    newBorn.BirthYear = year;
                    newBorn.NumberOfChildren = 0;
                    newBorn.Gender = (Gender)(rng.Next(1, 3));
                    People.Add(newBorn);
                }
            }
        }

        public List<Person> GetPeople(string file)
        {
            List<Person> p = new List<Person>();

            using (StreamReader sr = new StreamReader(file, Encoding.Default))
            {
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    p.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NumberOfChildren = byte.Parse(line[2])
                    });
                }
            }

            return p;
        }

        public List<BirthProbability> GetBirthProbabilities(string file)
        {
            List<BirthProbability> bp = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(file, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    bp.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NumberOfChildren = byte.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }
            }

            return bp;
        }

        public List<DeathProbability> GetDeathProbabilities(string file)
        {
            List<DeathProbability> dp = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(file, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    dp.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }
            }

            return dp;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            NumberOfMales.Clear();
            NumberOfFemales.Clear();
            Simulation();
            DisplayResults();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            else
                textBoxPath.Text = ofd.FileName;
        }
    }
}
