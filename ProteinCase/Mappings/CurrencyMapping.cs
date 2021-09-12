using System;
using Mapster;
using ProteinCase.Entities;

namespace ProteinCase.Mappings
{
    public class CurrencyMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CurrencyModel, Currency>()
                .Map(currency => currency.BanknoteBuying,
                    model => string.IsNullOrWhiteSpace(model.BanknoteBuying)
                        ? default
                        : Convert.ToDecimal(model.BanknoteBuying))
                .Map(currency => currency.BanknoteSelling,
                    model => string.IsNullOrWhiteSpace(model.BanknoteSelling)
                        ? default
                        : Convert.ToDecimal(model.BanknoteSelling))
                .Map(currency => currency.ForexSelling,
                    model => string.IsNullOrWhiteSpace(model.ForexSelling)
                        ? default
                        : Convert.ToDecimal(model.ForexSelling))
                .Map(currency => currency.ForexBuying,
                    model => string.IsNullOrWhiteSpace(model.ForexBuying)
                        ? default
                        : Convert.ToDecimal(model.ForexBuying))
                .Map(currency => currency.CrossRateUSD,
                    model => string.IsNullOrWhiteSpace(model.CrossRateUSD)
                        ? default
                        : Convert.ToDecimal(model.CrossRateUSD))
                .Map(currency => currency.CrossRateOther,
                    model => string.IsNullOrWhiteSpace(model.CrossRateOther)
                        ? default
                        : Convert.ToDecimal(model.CrossRateOther));
        }
    }
}