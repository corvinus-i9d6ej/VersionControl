﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Webszolgaltatas_I9D6EJ.MnbServiceReference;
using Webszolgaltatas_I9D6EJ.Entities;
using System.Xml;

namespace Webszolgaltatas_I9D6EJ
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;

            dataGridView1.DataSource = Rates;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement e in xml.DocumentElement)
            {
                var rate = new RateData();
                rate.Date = DateTime.Parse(e.GetAttribute("date"));

                var childElement = (XmlElement)e.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0) rate.Value = value / unit;
            }
        }
    }
}
