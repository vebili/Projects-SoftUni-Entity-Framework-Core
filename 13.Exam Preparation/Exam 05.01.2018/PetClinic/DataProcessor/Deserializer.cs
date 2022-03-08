namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using Dtos.ImportDtos;
    using Models;
    using Newtonsoft.Json;
    using PetClinic.Data;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAidsDto = JsonConvert.DeserializeObject<List<AnimalAidImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            var dbAnimalAidsNames = context
                .AnimalAids
                .Select(aa => aa.Name)
                .ToList();

            var validAnimalAids = new List<AnimalAid>();

            foreach (var dto in animalAidsDto)
            {
                if (!IsValid(dto) || validAnimalAids
                        .Any(a => a.Name == dto.Name))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animalAid = new AnimalAid
                {
                    Name = dto.Name,
                    Price = dto.Price
                };

                if (!dbAnimalAidsNames.Contains(animalAid.Name))
                {
                    validAnimalAids.Add(animalAid);
                }
                else
                {
                    continue;
                }

                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }

            context.AnimalAids.AddRange(validAnimalAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animalDto = JsonConvert.DeserializeObject<List<AnimalImportDto>>(jsonString);

            StringBuilder sb = new StringBuilder();

            var dbAnimalNames = context
                .Animals
                .Select(a => a.Name)
                .ToList();

            var validAnimals = new List<Animal>();

            foreach (var dto in animalDto)
            {
                if (!IsValid(dto)
                    || !IsValid(dto.Passport)
                    || validAnimals.Any(a =>
                                          a.Passport.SerialNumber
                                          == dto.Passport.SerialNumber))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var animal = new Animal
                {
                    Name = dto.Name,
                    Type = dto.Type,
                    Age = dto.Age,
                    Passport = new Passport
                    {
                        SerialNumber = dto.Passport.SerialNumber,
                        OwnerPhoneNumber = dto.Passport.OwnerPhoneNumber,
                        OwnerName = dto.Passport.OwnerName,
                        RegistrationDate = DateTime.ParseExact(dto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    }
                };

                animal.Passport.Animal = animal;

                if (!dbAnimalNames.Contains(animal.Name))
                {
                    validAnimals.Add(animal);
                }
                else
                {
                    continue;
                }

                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }

            context.Animals.AddRange(validAnimals);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Vets");
            var serializer = new XmlSerializer(typeof(List<VetImportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var validVets = new List<Vet>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var vetsDto = (List<VetImportDto>)serializer.Deserialize(reader);

                foreach (var dto in vetsDto)
                {
                    if (!IsValid(dto) || validVets.Any(v => v.PhoneNumber == dto.PhoneNumber))
                    {
                        sb.AppendLine("Error: Invalid data.");
                        continue;
                    }

                    var vet = new Vet
                    {
                        Name = dto.Name,
                        Profession = dto.Profession,
                        Age = dto.Age,
                        PhoneNumber = dto.PhoneNumber
                    };

                    if (context.Vets.All(v => v.PhoneNumber != vet.PhoneNumber))
                    {
                        validVets.Add(vet);
                    }
                    else
                    {
                        continue;
                    }

                    sb.AppendLine($"Record {vet.Name} successfully imported.");
                }
            }

            context.Vets.AddRange(validVets);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var attr = new XmlRootAttribute("Procedures");
            var serializer = new XmlSerializer(typeof(List<ProcedureImportDto>), attr);

            StringBuilder sb = new StringBuilder();

            var validProcedures = new List<Procedure>();

            using (StringReader reader = new StringReader(xmlString))
            {
                var proceduresDto = (List<ProcedureImportDto>)serializer.Deserialize(reader);

                foreach (var dto in proceduresDto)
                {
                    var vet = context
                        .Vets
                        .FirstOrDefault(v => v.Name == dto.Vet);

                    var animal = context
                        .Animals
                        .FirstOrDefault(a => a.PassportSerialNumber == dto.Animal);

                    var allAidsExists = true;
                    var validProcedureAnimalAids = new List<ProcedureAnimalAid>();

                    foreach (var dtoAnimalAid in dto.AnimalAids)
                    {
                        var animalAid = context
                            .AnimalAids
                            .FirstOrDefault(aa => aa.Name == dtoAnimalAid.Name);
                        if (animalAid == null 
                            || validProcedureAnimalAids
                                .Any(paa => paa.AnimalAid.Name 
                                            == dtoAnimalAid.Name))
                        {
                            allAidsExists = false;
                            break;
                        }

                        var animalAidProcedure = new ProcedureAnimalAid
                        {
                            AnimalAid = animalAid
                        };

                        validProcedureAnimalAids.Add(animalAidProcedure);
                    }

                    if (!IsValid(dto)
                        || !dto.AnimalAids.All(IsValid)
                        || vet == null
                        || animal == null
                        || !allAidsExists)
                    {
                        sb.AppendLine("Error: Invalid data.");
                        continue;
                    }

                    var procedure = new Procedure
                    {
                        Animal = animal,
                        Vet = vet,
                        DateTime = DateTime.ParseExact(dto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        ProcedureAnimalAids = validProcedureAnimalAids
                    };

                    validProcedures.Add(procedure);
                    sb.AppendLine($"Record successfully imported.");
                }
            }

            context.Procedures.AddRange(validProcedures);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}
