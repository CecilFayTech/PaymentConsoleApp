using CsvHelper.Configuration;

namespace PaymentConsoleApp
{
    public  class BeneficiariesMap : ClassMap<BeneficiaryDetails>
    {
        public BeneficiariesMap()
        {
            Map(b => b.ID).Index(0);
            Map(b => b.FullName).Index(1);
            Map(b => b.AccountNumber).Index(2);
            Map(b => b.EmailAddress).Index(3);
        }
    }
}
