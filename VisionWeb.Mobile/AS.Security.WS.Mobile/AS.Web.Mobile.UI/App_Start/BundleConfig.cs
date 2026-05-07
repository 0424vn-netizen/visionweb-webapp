using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AS.Web.Mobile.UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                "~/Scripts/lib/jquery-{version}.min.js",
                "~/Scripts/lib/jquery-migrate-{version}.min.js"
            ));

            //Kendo chart js
            bundles.Add(new ScriptBundle("~/js/kendo-chart").Include(
                "~/Scripts/kendo/kendo.data.min.js",
                "~/Scripts/kendo/kendo.userevents.min.js",
                "~/Scripts/kendo/kendo.color.min.js",
                "~/Scripts/kendo/kendo.popup.min.js",
                "~/Scripts/kendo/kendo.drawing.min.js",
                "~/Scripts/kendo/kendo.dataviz.core.min.js",
                "~/Scripts/kendo/kendo.dataviz.themes.min.js",
                "~/Scripts/kendo/kendo.dataviz.chart.min.js"
            ));

            //Kendo window
            bundles.Add(new ScriptBundle("~/js/kendo-window").Include(
                "~/Scripts/kendo/kendo.core.min.js",
               "~/Scripts/kendo/kendo.draganddrop.min.js",
               "~/Scripts/kendo/kendo.window.min.js"
           ));

            //Kendo calendar
            bundles.Add(new ScriptBundle("~/js/kendo-calendar").Include(
               "~/Scripts/kendo/kendo.calendar.min.js",
               "~/Scripts/datefilter.js"
           ));

            // Kendo dropdownlist
            bundles.Add(new ScriptBundle("~/js/kendo-dropdownlist").Include(
              "~/Scripts/kendo/kendo.data.min.js",
              "~/Scripts/kendo/kendo.popup.min.js",
              "~/Scripts/kendo/kendo.list.min.js",
              "~/Scripts/kendo/kendo.dropdownlist.min.js"));

            bundles.Add(new ScriptBundle("~/js/common").Include(
                "~/Scripts/lib/mustache.js",
                "~/Scripts/lib/headroom.js",
                "~/Scripts/lib/as.events.js",
                "~/Scripts/lib/as.infinityScroll.js",
                "~/Scripts/app.js"
            ));

            //ViewFullSite
            bundles.Add(new ScriptBundle("~/js/viewfullsite").Include(
                "~/Scripts/viewfullsite.js"
            ));

            bundles.Add(new ScriptBundle("~/js/validation").Include(
                "~/Scripts/lib/jquery.validate.js",
                "~/Scripts/lib/jquery.validate.unobtrusive.js",
                "~/Scripts/lib/as.customvalidation.js"
            ));

            //Bath
            bundles.Add(new ScriptBundle("~/js/app-batch").Include(
                "~/Scripts/batch.js"
            ));

            //Retrieval
            bundles.Add(new ScriptBundle("~/js/app-retrieval").Include(
                "~/Scripts/retrieval.js"
            ));

            //RetrievalByDate
            bundles.Add(new ScriptBundle("~/js/app-retrievalbydate").Include(
                "~/Scripts/retrievalbydate.js"
            ));

            //Chargeback
            bundles.Add(new ScriptBundle("~/js/app-chargeback").Include(
                "~/Scripts/chargeback.js"
            ));

            //ChargebackByDate
            bundles.Add(new ScriptBundle("~/js/app-chargebackbydate").Include(
                "~/Scripts/chargebackbydate.js"
            ));

            //Deposit
            bundles.Add(new ScriptBundle("~/js/app-deposit").Include(
                "~/Scripts/deposit.js"
            ));

            //Dashboard
            bundles.Add(new ScriptBundle("~/js/app-dashboard").Include(
                "~/Scripts/dashboard.js"
            ));

            //Statement
            bundles.Add(new ScriptBundle("~/js/app-statement").Include(
                "~/Scripts/statement.js"
            ));

            //UpdateProfile
            bundles.Add(new ScriptBundle("~/js/profile").Include(
                "~/Scripts/updatemyprofile.js"
            ));

            //Styles
            bundles.Add(new StyleBundle("~/content/css/fonts").Include(
                "~/Content/css/fonts.css"
            ));

            //Float label
            bundles.Add(new ScriptBundle("~/js/float-label").Include(
                "~/Scripts/floatLabel.js"
            ));

            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();

            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}