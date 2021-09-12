using System;
using System.Xml.Serialization;

namespace ProteinCase
{
    [XmlRoot("Tarih_Date")]
    public class CurrencyResponseModel
    {
        [XmlElement("Currency")] public CurrencyModel[] Currencies { get; set; }
    }

    [Serializable]
    public class CurrencyModel
    {
        [System.Xml.Serialization.XmlAttribute("CrossOrder")]
        public int CrossOrder { get; set; }

        [System.Xml.Serialization.XmlAttribute("Kod")]
        public string Kod { get; set; }

        [XmlAttribute("CurrencyCode")] public string CurrencyCode { get; set; }

        [System.Xml.Serialization.XmlElement("Unit")]
        public int Unit { get; set; }

        [System.Xml.Serialization.XmlElement("Isim")]
        public string Isim { get; set; }

        [System.Xml.Serialization.XmlElement("CurrencyName")]
        public string CurrencyName { get; set; }

        [System.Xml.Serialization.XmlElement("ForexBuying")]
        public string ForexBuying { get; set; }

        [System.Xml.Serialization.XmlElement("ForexSelling")]
        public string ForexSelling { get; set; }

        [System.Xml.Serialization.XmlElement("BanknoteBuying")]
        public string BanknoteBuying { get; set; }

        [System.Xml.Serialization.XmlElement("BanknoteSelling")]
        public string BanknoteSelling { get; set; }

        [System.Xml.Serialization.XmlElement("CrossRateUSD")]
        public string CrossRateUSD { get; set; }

        [System.Xml.Serialization.XmlElement("CrossRateOther")]
        public string CrossRateOther { get; set; }
    }
}