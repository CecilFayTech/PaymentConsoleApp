using CsvHelper;
using System.Globalization;

namespace PaymentConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool hasExited = false;
            bool showMainMenu = true;
            string? selectedMenuOption = string.Empty;
            string? beneficiaryOption = string.Empty;
            List<BeneficiaryDetails> headerObject = new List<BeneficiaryDetails>();
            CsvManager.CreateDefaultUserDetails();
            CsvManager.AddRecordToCSVFile(headerObject); 

            Console.WriteLine("\nWELCOME TO THE PAYMENT CONSOLE");
            do
            {            
                if (showMainMenu)
                {
                    bool validationIncomplete = true;

                    DisplayMainMenu();

                    while (validationIncomplete)
                    {
                        selectedMenuOption = Console.ReadLine();

                        validationIncomplete = Validation.ValidateMainMenu(selectedMenuOption);
                    }                  
                }

                if (selectedMenuOption == "1")
                {
                    CsvManager.ViewDefaultUserDetails();

                    Console.WriteLine("\nPress Z to return to Main Menu , or ESC to exit\n");
                    
                    ConsoleKeyInfo key = Console.ReadKey();

                    var keyMapping = KeyInputMapper(key);                           
                    showMainMenu = keyMapping.Item3;
                    hasExited = keyMapping.Item4;
                }

                if (selectedMenuOption == "2")
                {
                    showMainMenu= false;

                    bool validationIncomplete2 = true;
                    
                    DisplaySecondaryMenu();
                
                    while (validationIncomplete2)
                    {
                        beneficiaryOption = Console.ReadLine();

                        validationIncomplete2 = Validation.ValidateMenu2(beneficiaryOption);
                    }                  

                    if (beneficiaryOption == "1")
                    {
                        List<BeneficiaryDetails> tmp = new List<BeneficiaryDetails>();
                        if (!File.Exists("fileBeneficiaries.csv"))
                        {
                            CsvManager.AddRecordToCSVFile(tmp);

                            Console.WriteLine("\nNo Beneficiaries Available");
                        }
                        else
                        
                        ShowBeneficiaries();
                                                
                        Console.WriteLine("\nPress SPACEBAR to Add Beneficiary");                     
                        Console.WriteLine("Press BACKSPACE to return to Menu 2 , Press Z to go back to Main Menu or ESC to exit");
                        Console.WriteLine();

                        ConsoleKeyInfo key = Console.ReadKey();

                        var keyMapping = KeyInputMapper(key);                           
                            selectedMenuOption = keyMapping.Item1;
                            beneficiaryOption = keyMapping.Item2;
                            showMainMenu = keyMapping.Item3;
                            hasExited = keyMapping.Item4;                       
                    }

                    if (beneficiaryOption == "2")
                    {
                        List<BeneficiaryDetails> tmp = new List<BeneficiaryDetails>();
                        var beneficiariesInFile = CsvManager.GetBeneficiariesFromCSVFile();

                        tmp.Add(CaptureBeneficiary(beneficiariesInFile));
                                             
                        Console.WriteLine("\nPress ENTER to submit Beneficiary");

                        CsvManager.AppendToCsvFile(tmp);

                        if (Console.ReadKey().Key == ConsoleKey.Enter)                       
                        {
                            ShowBeneficiaries();
                            Console.WriteLine("\nBeneficiary Added Successfully");   
                        }                        

                        Console.WriteLine("\nPress BACKSPACE to return to Menu 2 , Press Z to go to Main Menu or ESC to exit");
                        
                        ConsoleKeyInfo key = Console.ReadKey();

                        var keyMapping = KeyInputMapper(key);
                        selectedMenuOption = keyMapping.Item1;
                        showMainMenu = keyMapping.Item3;
                        hasExited = keyMapping.Item4;
                    }
                    
                    if (beneficiaryOption == "3")
                    {
                        showMainMenu = true;
                    }

                    if (beneficiaryOption == "4")
                    {
                        List<BeneficiaryDetails> beneficiaries = CsvManager.GetBeneficiariesFromCSVFile();
                        Guid guid = new Guid();
                        ShowBeneficiaries();

                        bool validationIncomplete3 = true;
                        
                        if (beneficiaries != null && beneficiaries.Any())
                        {
                            Console.WriteLine("\nCopy and paste the Id of the beneficiary you want to delete");

                            while (validationIncomplete3)
                            {
                                var beneficiaryId = Console.ReadLine();


                                var result = Validation.DeleteBeneficiaryValidation(beneficiaryId);

                                validationIncomplete3 = result.Item1.IsInvalid;

                                if (result.Item1.IsInvalid)
                                {
                                    foreach (var message in result.Item1.IsInvalidtext)
                                    {
                                        Console.WriteLine(message);
                                    }
                                }

                                if (!result.Item1.IsInvalid)
                                {
                                    guid = result.Item2;
                                }

                            }

                            if (beneficiaries.Any())
                            {
                                var result = CsvManager.DeleteBeneficiary(guid); // pass in guid

                                if (result)
                                {
                                    Console.WriteLine("\nBeneficiary deleted sucessfully");
                                    ShowBeneficiaries();
                                }
                                else
                                {
                                    Console.WriteLine("\nBeneficiary not deleted!");
                                }
                            }
                            else 
                            {
                                Console.WriteLine("No beneficiaries to delete");
                            }
                        }
                
                        Console.WriteLine("\nPress BACKSPACE to return to Menu 2 , Press Z to go to Main Menu or ESC to exit");

                        ConsoleKeyInfo key = Console.ReadKey();

                        var keyMapping = KeyInputMapper(key);
                        selectedMenuOption = keyMapping.Item1;                       
                        showMainMenu = keyMapping.Item3;
                        hasExited = keyMapping.Item4;
                    }                                                             
                }             
            } while (!hasExited);
            File.Delete("fileBeneficiaries.csv");
        }
       
       static void ShowBeneficiaries()
        {
            var beneficiaries = CsvManager.GetBeneficiariesFromCSVFile(); 
            if (beneficiaries.Any())
            {
                using (var reader = new StreamReader("fileBeneficiaries.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                
                {
                    csv.Context.RegisterClassMap<BeneficiariesMap>();
                    var records = csv.GetRecords<BeneficiaryDetails>().ToList();

                    foreach (var record in records)
                    {
                        Console.WriteLine("__________________________________\n");
                        Console.WriteLine("ID: " + record.ID);
                        Console.WriteLine("FullName: " + record.FullName);
                        Console.WriteLine("AccountNumber: " + record.AccountNumber);
                        Console.WriteLine("EmailAddres: " + record.EmailAddress);
                    }
                }               
            }
            else
            {
                {
                    Console.WriteLine("\nNo Beneficiaries Available");
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\nMain Menu \n");
            Console.WriteLine("Please choose an option and press enter:");
            Console.WriteLine("1. View User Details");
            Console.WriteLine("2. Manage Beneficiaries");
        }

        static void DisplaySecondaryMenu()
        {
            Console.WriteLine("\nMenu 2\n ");
            Console.WriteLine("Please choose an option and press enter:");
            Console.WriteLine("1. View all Beneficiaries");
            Console.WriteLine("2. Add Beneficiary");
            Console.WriteLine("3. Go back to Main Menu");
            Console.WriteLine("4. Delete Beneficiary");
        }

        static BeneficiaryDetails CaptureBeneficiary(List<BeneficiaryDetails> beneficiariesInFile)
        {
            BeneficiaryDetails beneficiary = new BeneficiaryDetails();

            beneficiary.ID= Guid.NewGuid();
            Console.WriteLine("\nBeneiciary ID: " + beneficiary.ID);
            
            Console.WriteLine("\nPlease enter Full Name:");
            
            bool namevalidationIncomplete = true;

            while (namevalidationIncomplete)
            {
                beneficiary.FullName = string.Empty; 
                beneficiary.FullName = Console.ReadLine();

                ValidationResults results = Validation.IsValidFullName(beneficiary.FullName,beneficiariesInFile);
                namevalidationIncomplete = results.IsInvalid;

                foreach (var errorMessage in results.IsInvalidtext)
                {
                    Console.WriteLine(errorMessage);
                }

                results.IsInvalidtext = new List<string>();
            }

            Console.WriteLine("\nPlease enter Account Number:");

            bool accountNumberValidationIncomplete = true;

            while (accountNumberValidationIncomplete)
            {
                beneficiary.AccountNumber = string.Empty;
                beneficiary.AccountNumber = Console.ReadLine();

                ValidationResults results1 = Validation.IsValidAccountNumber(beneficiary.AccountNumber, beneficiariesInFile);
                accountNumberValidationIncomplete = results1.IsInvalid;

                foreach (var errorMessage in results1.IsInvalidtext)
                {
                    Console.WriteLine(errorMessage);
                }
            }

            Console.WriteLine("\nPlease enter Email Address:");
                       
            bool emailaddressValidtionIncomplete = true;

            while (emailaddressValidtionIncomplete)
            {
                beneficiary.EmailAddress = string.Empty;
                beneficiary.EmailAddress = Console.ReadLine();

                ValidationResults results2 = Validation.IsValidEmailAddress(beneficiary.EmailAddress, beneficiariesInFile);
                emailaddressValidtionIncomplete = results2.IsInvalid;

                foreach (var errorMessage in results2.IsInvalidtext)
                {
                    Console.WriteLine(errorMessage);
                }
            }            
            return beneficiary;                       
        }
       
        static (string ,string, bool,bool) KeyInputMapper(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                return ("2","",false,false);
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                return ("", "2", false, false);
            }
            else if (key.Key == ConsoleKey.Z)
            {
                return ("", "", true, false);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return ("", "", false, true);
            }
            return ("", "", true, false);
        }        
    }
}