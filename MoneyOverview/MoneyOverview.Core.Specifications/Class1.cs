using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace MoneyOverview.Core.Specifications
{
    [Subject("IN")]
    class When_Income_Is_Not_Registered
    {
        //static Dummy invoice;

        Establish the_context = () =>
        {
            //List<string> l = new List<string>();
            //l.Add("test");

        };

        //Because of = () =>
        //    invoice = new Dummy();

        It has_no_sales_tax_added;
        //= () =>
        //    invoice.ShouldBeOfType<Dummy>();

        //Not implemented:
        It should_default_published_date_to_now;
    }

    
}
