using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeneticAlgorithm.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GeneticAlgorithm.Models
{
    public class Evolution
    {
        public ObjectId Id { get; set; }
        public List<MemberModel> Population { get; set; }
        public MemberModel Target { get; set; }
        public int ValueCount { get; set; }
        public int VariableCount { get; set; }
        public int Generation { get; set; }
        public bool Done { get; set; }
        public int NewMemberPerGeneration { get; set; }
        public int TotalMembers { get; set; }
        public List<int> History { get; set; }

        public Evolution(int populationCount, int variableCount, int valueCount, Random random)
        {
            Id = ObjectId.GenerateNewId();
            ValueCount = valueCount;
            VariableCount = variableCount;
            Population = new List<MemberModel>();
            Generation = 0;
            Done = false;
            TotalMembers = populationCount;

            Target = new MemberModel();
            Target.Variables = new List<int>();
            History = new List<int>();

            for (var j = 0; j < variableCount; j++)
            {
                var value = random.Next(0, valueCount);
                Target.Variables.Add(value);
            }

            for (var i = 0; i < populationCount; i++)
            {
                var member = new MemberModel();
                member.Variables = new List<int>();

                for (var j = 0; j < variableCount; j++)
                {
                    var value = random.Next(0, valueCount);
                    member.Variables.Add(value);
                }

                member.SetFitness(Target);
                Population.Add(member);
            }

            Population = Population.OrderByDescending(p => p.Fitness).ToList();

            Insert();
        }

        public Evolution()
        {

        }
        public void Save()
        {
            var collection = DatabaseHelpers.GetSeedCollection();
            var filter = Builders<Evolution>.Filter.Eq(s => s.Id, this.Id);
            collection.ReplaceOne(filter, this);
        }

        public void Insert()
        {
            var collection = DatabaseHelpers.GetSeedCollection();
            collection.InsertOne(this);
        }

        public void Evolve(Random random)
        {
            if (!Done)
            {
                EvolutionStep(random);
                Save();
            }
        }

        public void EvolveToEnd(Random random)
        {
            while (!Done)
            {
                EvolutionStep(random);
            }
            Save();
        }

        public void EvolutionStep(Random random)
        {
            for (var i = 0; i < Population.Count; i++)
            {
                Population[i].SetFitness(Target);
            }

            var newBabyCount = (int)Population.Count / 2;
            TotalMembers += newBabyCount;

            for (var i = 0; i < newBabyCount; i++)
            {
                Population[Population.Count - i - 1] = Population[i].Couplement(Population[i + 1]);
                Population[Population.Count - i - 1].Mutate(ValueCount, random);
                Population[Population.Count - i - 1].SetFitness(Target);
            }

            Population = Population.OrderByDescending(p => p.Fitness).ToList();
            Generation++;
            History.Add(Population[0].Fitness);
            if (Population[0].Fitness == VariableCount)
                Done = true;
            System.Diagnostics.Debug.WriteLine(History[History.Count - 1]);

        }
    }

    public class MemberModel
    {
        public List<int> Variables { get; set; }
        public int Fitness { get; set; }

        public MemberModel()
        {
            Variables = new List<int>();
        }

        public void SetFitness(MemberModel target)
        {
            Fitness = 0;

            for (var i = 0; i < Variables.Count; i++)
            {
                if (Variables[i] == target.Variables[i])
                    Fitness++;
            }
        }

        public void Mutate(int valueCount, Random random)
        {
            var mutationIndex = random.Next(0, Variables.Count);
            Variables[mutationIndex] = random.Next(0, valueCount);
        }

        public MemberModel Couplement(MemberModel mate)
        {
            var baby = new MemberModel();

            var random = new Random();

            int crossIndex = random.Next(0, Variables.Count);
            bool activeParent = random.Next(0, 1) % 2 == 0;

            for (var i = 0; i < Variables.Count; i++)
            {
                if (activeParent)
                {
                    if (i < crossIndex)
                        baby.Variables.Add(Variables[i]);
                    else
                        baby.Variables.Add(mate.Variables[i]);
                }
                else
                {
                    if (i < crossIndex)
                        baby.Variables.Add(mate.Variables[i]);
                    else
                        baby.Variables.Add(Variables[i]);
                }
            }

            return baby;
        }

    }

    public class EvolvePost
    {
        public string evolutionId { get; set; }
    }
}