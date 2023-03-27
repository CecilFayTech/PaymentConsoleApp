using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace PaymentConsoleApp
{
    public static class CsvManager
    {
        public static void CreateDefaultUserDetails()
        {
            var userDetails = PopulateUser();

            using (var writer = new StreamWriter("userdetails.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))

            {
                csv.WriteRecords(userDetails);
            }
        }

        private static List<UserDetails> PopulateUser()
        {
            List<UserDetails> userList = new List<UserDetails>();

            UserDetails user = new UserDetails();
            user.FullName = "Ratidzo Gunguwo";
            user.CellNumber = "0615162503";

            userList.Add(user);
            return userList;
        }

        public static void ViewDefaultUserDetails()
        {
            using (var reader = new StreamReader("userdetails.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<UserDetails>().ToList();

                foreach (var record in records)
                {
                    Console.WriteLine("___________________________________________\n");
                    Console.WriteLine("FullName: " + record.FullName);
                    Console.WriteLine("CellNumber: " + record.CellNumber);
                }
            }
        }

        public static List<BeneficiaryDetails> GetBeneficiariesFromCSVFile()
        {
            List<BeneficiaryDetails> beneficiaries = new List<BeneficiaryDetails>();

            using (var reader = new StreamReader("fileBeneficiaries.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))

            {
                csv.Context.RegisterClassMap<BeneficiariesMap>();
                var records = csv.GetRecords<BeneficiaryDetails>().ToList();
                beneficiaries = records;
            }
            return beneficiaries;
        }

        public static bool DeleteBeneficiary(Guid idTodelete)
        { 
           bool isFileDeleted = false;
            
                var tmp = GetBeneficiariesFromCSVFile();

                var itemtoDelete = tmp.Where(x => x.ID == idTodelete).FirstOrDefault();

                tmp.Remove(itemtoDelete);
                isFileDeleted = true;

                CsvManager.AddRecordToCSVFile(tmp);
                return isFileDeleted;
        }

        public static void AddRecordToCSVFile(List<BeneficiaryDetails> tmp)
        {
            var HasHeaderRecord = string.Format("ID ", "FullName ", "AccountNumber ", "EmailAddress");
            var config1 = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };
            using (var writer = new StreamWriter("fileBeneficiaries.csv"))
            using (var csv = new CsvWriter(writer, config1))
            {
                csv.Context.RegisterClassMap<BeneficiariesMap>();
                csv.WriteRecords(tmp);
            }
        }
        public static void AppendToCsvFile(List<BeneficiaryDetails> tmp)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using (var stream = File.Open("fileBeneficiaries.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(tmp);
            }
        }
    }   
}  

      


