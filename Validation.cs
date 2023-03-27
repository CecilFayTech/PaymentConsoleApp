using System.Text.RegularExpressions;

namespace PaymentConsoleApp
{
    public static class Validation
    {
        public static bool ValidateMainMenu(string?  selectedMenuOption)
        {
            bool invalidMainmenu = true;  

            char selection = selectedMenuOption.ToCharArray().FirstOrDefault(); 

            if (!int.TryParse(selectedMenuOption, out var number) && char.IsNumber(selection))     
            {
                return invalidMainmenu;
            }
            
            if ( number == 1 || number == 2) 
            {
                invalidMainmenu = false;  
                Console.WriteLine("\nThank you Proceeding to option " + number);
                return invalidMainmenu;
            }
                
            Console.WriteLine("Invalid Input! Please try again.");
  
            return invalidMainmenu;
        }
        public static bool ValidateMenu2(string beneficiaryOption)
        {
            bool invalidMenu2 = true;
           
            char selection2 = beneficiaryOption.ToCharArray().FirstOrDefault(); 

            if (!int.TryParse(beneficiaryOption, out var benNumber) && char.IsNumber(selection2))    
            {
                return invalidMenu2;
            }

            if (benNumber == 1 || benNumber == 2 || benNumber == 3 || benNumber == 4) 
            {
                invalidMenu2 = false;  
                Console.WriteLine("Thank you Proceeding to option " + benNumber);
                return invalidMenu2;
            }

            Console.WriteLine("Invalid Input! Please try again.");

            return invalidMenu2;
        }
        public static ValidationResults IsValidFullName(string? FullName, List<BeneficiaryDetails> beneficiariesInFile)
        {
            ValidationResults nameValidation = new ValidationResults();
            nameValidation.IsInvalid = false;
            bool isInvalidCharacter = false;
            bool isduplicatename = false;
            
            if (string.IsNullOrEmpty(FullName))
            {
                nameValidation.IsInvalid = true;
                nameValidation.IsInvalidtext.Add("Full name must not be empty!");
                return nameValidation;
            }

            var names = FullName.Split(" ");

            foreach ( var name in names) 
            {
                if (beneficiariesInFile.Any(x => x.FullName.Contains(name) ))
                {
                    nameValidation.IsInvalid = true;
                    isduplicatename = true;
                }

                foreach (char c in name)
                {
                    if (!char.IsLetter(c))
                    {
                        nameValidation.IsInvalid = true;
                        isInvalidCharacter = true;
                    }
                }
            }

            if (isduplicatename)
            {
                nameValidation.IsInvalidtext.Add("Beneficiary Name already exists! Enter new beneficiary name");
            }

            if (isInvalidCharacter)
            {
                nameValidation.IsInvalidtext.Add("Invalid Input! Use letters only");
            }
            
            if (names.Length > 50 || names.Length < 2)
            {
                nameValidation.IsInvalid = true;
                nameValidation.IsInvalidtext.Add("Invalid Input! Please enter between 2 and 50 letters");
            } 
            
            return nameValidation;
        }

        public static ValidationResults IsValidAccountNumber(string? AccountNumber, List<BeneficiaryDetails> beneficiariesInFile)
        {
            ValidationResults accountNumberValidation = new ValidationResults();
            
            accountNumberValidation.IsInvalid= false;
 
            if (string.IsNullOrEmpty(AccountNumber))
            {
                accountNumberValidation.IsInvalid = true;
            }

            foreach (char c in AccountNumber)
            {
                if (!char.IsNumber(c))
                    accountNumberValidation.IsInvalid = true;               
            }
            
            if (accountNumberValidation.IsInvalid)
            {
                accountNumberValidation.IsInvalidtext.Add("Invalid input! Use numbers only");
            }

            if (AccountNumber.Length != 13)
            {
                accountNumberValidation.IsInvalid = true;
                accountNumberValidation.IsInvalidtext.Add("Invalid input! Account Number must be 13 numbers!");                
            }
            if (beneficiariesInFile.Any(y => y.AccountNumber == AccountNumber))
            {
                accountNumberValidation.IsInvalid = true;
                accountNumberValidation.IsInvalidtext.Add("Account Number already exists! Enter new Account Number");
            }
            return accountNumberValidation;
        }
        
        public static ValidationResults IsValidEmailAddress(string EmailAddress, List<BeneficiaryDetails> beneficiariesInFile)
        {
            ValidationResults emailValidation = new ValidationResults();
            
            emailValidation.IsInvalid = false;
            
            if (string.IsNullOrEmpty(EmailAddress))
            {
                emailValidation.IsInvalid = true;
            }

            if (emailValidation.IsInvalid)
            {

                emailValidation.IsInvalidtext.Add("Email Address field cannot be empty");
            }

            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]
                            {1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            
            Regex regex = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.Singleline);

            if (!regex.IsMatch(EmailAddress)) 
            {
                emailValidation.IsInvalid = true;
                emailValidation.IsInvalidtext.Add("Invalid Input! Enter correct email address");
            }
            if (beneficiariesInFile.Any(z => z.EmailAddress == EmailAddress))
            {
                emailValidation.IsInvalid = true;
                emailValidation.IsInvalidtext.Add("Email Address already exists! Enter new email address");
            }
            return emailValidation;
        }

        public static (ValidationResults, Guid) DeleteBeneficiaryValidation(string Id)
        {
            ValidationResults validationResults = new ValidationResults();
            validationResults.IsInvalid = true;

            var isGuid = Guid.TryParse(Id, out Guid guidtoreturn);

            if (isGuid)
            {
                validationResults.IsInvalid = false;
            }
            if(!isGuid)
            {
                validationResults.IsInvalid = true;
                validationResults.IsInvalidtext.Add("Invalid Input! Enter valid ID");
            }
            return (validationResults, guidtoreturn);           
        }
    }   
}






    
