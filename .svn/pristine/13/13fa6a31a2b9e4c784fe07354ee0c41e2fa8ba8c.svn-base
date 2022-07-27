using System.Web;
using System.Web.Optimization;

namespace ProjectBase
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/jquery/js").Include(
                     "~/Content/adminlte/bower_components/jquery/dist/jquery.min.js"
            ));
            bundles.Add(new StyleBundle("~/print/css").Include(
                      "~/Content/adminlte/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/adminlte/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/adminlte/bower_components/Ionicons/css/ionicons.min.css"
            ));
            bundles.Add(new StyleBundle("~/adminlte_dashboard/css").Include(
                 "~/Content/adminlte/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/adminlte/dist/css/AdminLTE.min.css",

                      //"~/Content/fontawesome-free-5.9.0-web/css/all.min.css",
                      "~/Content/adminlte/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/adminlte/bower_components/Ionicons/css/ionicons.min.css",
                      "~/Content/adminlte/dist/css/skins/_all-skins.min.css",
                       "~/Content/adminlte/bower_components/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                      "~/Content/adminlte/bower_components/morris.js/morris.css",
                      "~/Content/adminlte/bower_components/jvectormap/jquery-jvectormap.css",
                      "~/Content/adminlte/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.css",
                       "~/Content/adminlte/bower_components/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css",
                       "~/Content/adminlte/plugins/timepicker/bootstrap-timepicker.min.css",
                       "~/Content/adminlte/bower_components/bootstrap-daterangepicker/daterangepicker.css",
                      "~/Scripts/select2-3.5.4/css/select2.css",
                      "~/Scripts/select2-3.5.4/css/select2_bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/google-font.css"
            ));
            bundles.Add(new StyleBundle("~/adminlte_min/css").Include(
                    "~/Content/adminlte/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/adminlte/dist/css/AdminLTE.min.css",

                      //"~/Content/fontawesome-free-5.9.0-web/css/all.min.css",
                      "~/Content/adminlte/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/Content/adminlte/bower_components/Ionicons/css/ionicons.min.css",
                      "~/Content/adminlte/bower_components/DataTables-1.10.18/css/jquery.dataTables.min.css",

                      "~/Content/adminlte/dist/css/skins/_all-skins.min.css",
                      "~/Scripts/select2-3.5.4/css/select2.css",
                      "~/Scripts/select2-3.5.4/css/select2_bootstrap.css",
                      "~/Content/adminlte/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.css",
                       "~/Content/adminlte/bower_components/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css",
                       "~/Content/adminlte/plugins/timepicker/bootstrap-timepicker.min.css",
                       "~/Content/Site.css",
                      //"~/Content/adminlte/bower_components/bootstrap-daterangepicker/daterangepicker.css",
                      "~/Content/google-font.css",
                      "~/Content/progress-wizard.min.css"
            ));
            var print_bundle = new ScriptBundle("~/print/js").Include(
                     "~/Content/adminlte/bower_components/jquery/dist/jquery.min.js",
                     "~/Content/adminlte/bower_components/bootstrap/dist/js/bootstrap.min.js",
                     "~/Content/adminlte/bower_components/jquery-ui/jquery-ui.min.js",
                     "~/Content/adminlte/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                     "~/Content/adminlte/bower_components/jquery-slimscroll/fastclick/lib/fastclick.js",
                     "~/Scripts/jquery.number.js"
            );
            print_bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(print_bundle);

            var adminlte_dashboard_bundle = new ScriptBundle("~/adminlte_dashboard/js").Include(
                     "~/Content/adminlte/dist/js/adminlte.min.js",
                     "~/Content/adminlte/bower_components/bootstrap/dist/js/bootstrap.min.js",
                     "~/Content/adminlte/bower_components/jquery-ui/jquery-ui.min.js",
                     "~/Scripts/moment.min.js",
                     "~/Content/adminlte/bower_components/raphael/raphael.min.js",
                     "~/Content/adminlte/bower_components/morris.js/morris.min.js",
                     "~/Content/adminlte/bower_components/chart.js/Chart.js",
                     "~/Content/adminlte/bower_components/chart.js/chartjs-plugin-datalabels.min.js",
                     "~/Content/adminlte/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                     "~/Content/adminlte/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                     "~/Content/adminlte/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                     "~/Content/adminlte/bower_components/jquery-knob/dist/jquery.knob.min.js",
                     "~/Content/adminlte/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",

                     "~/Content/adminlte/bower_components/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js",
                     "~/Content/adminlte/plugins/timepicker/bootstrap-timepicker.min.js",


                     "~/Content/adminlte/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                     "~/Content/adminlte/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                     "~/Content/adminlte/bower_components/jquery-slimscroll/fastclick/lib/fastclick.js",

                     "~/Content/adminlte/bower_components/bootstrap-daterangepicker/daterangepicker.js",
                     "~/Content/adminlte/dist/js/pages/dashboard.js",
                     "~/Content/adminlte/dist/js/demo.js",
                     "~/Scripts/select2-3.5.4/js/select2.js",
                     "~/Scripts/jquery.number.js",
                     "~/Scripts/jquery-validation/dist/jquery.validate.js"
            );
            adminlte_dashboard_bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(adminlte_dashboard_bundle);

            var adminlte_min_bundle = new ScriptBundle("~/adminlte_min/js").Include(
                     "~/Content/adminlte/dist/js/adminlte.min.js",
                     "~/Content/adminlte/bower_components/bootstrap/dist/js/bootstrap.min.js",
                     "~/Content/adminlte/bower_components/jquery-ui/jquery-ui.min.js",


                     "~/Content/adminlte/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                     "~/Content/adminlte/bower_components/jquery-slimscroll/fastclick/lib/fastclick.js",
                     "~/Scripts/jquery.number.js",
                     "~/Scripts/jquery-validation/dist/jquery.validate.js",
                     "~/Content/adminlte/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                     "~/Scripts/moment.min.js",
                     "~/Content/adminlte/bower_components/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js",
                     "~/Content/adminlte/plugins/timepicker/bootstrap-timepicker.min.js",

                     "~/Content/adminlte/bower_components/DataTables-1.10.18/js/jquery.dataTables.min.js",
                     "~/Content/adminlte/bower_components/DataTables-1.10.18/js/dataTables.fixedColumns.min.js",

                     "~/Scripts/bootbox.min.js",
                     "~/Scripts/select2-3.5.4/js/select2.js",
                     "~/Scripts/jquery.blockUI.js"
            );
            adminlte_min_bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(adminlte_min_bundle);

            bundles.Add(new ScriptBundle("~/bundles/dataTable").Include(
                      "~/Scripts/datatable/dataTables.scroller.min.js",
                      "~/Scripts/datatable/export/dataTables.buttons.min.js",
                      "~/Scripts/datatable/export/jszip.min.js",
                      "~/Scripts/datatable/export/pdfmake.min.js",
                      "~/Scripts/datatable/export/vfs_fonts.js",
                      "~/Scripts/datatable/export/buttons.html5.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                        "~/Scripts/bootbox.min.js"));

            bundles.Add(new StyleBundle("~/css/my").Include(
                        "~/Content/my.css"));

            bundles.Add(new StyleBundle("~/report/css").Include(
                    "~/Content/adminlte/bower_components/bootstrap/dist/css/bootstrap.min.css",
                    "~/Content/adminlte/bower_components/font-awesome/css/font-awesome.min.css",
                    "~/Content/adminlte/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css",

                    "~/Content/Theme/adminlte/dist/css/AdminLTE.min.css",
                    "~/Content/Theme/adminlte/dist/css/skins/_all-skins.min.css",
                    "~/Content/Theme/adminlte/dist/css/fonts.css",
                    "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.dataTables.min.css",
                    "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.bootstrap.min.css",
                    "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.html5.min.css"
          ));

            bundles.Add(new Bundle("~/report/js").Include(
            "~/Content/adminlte/bower_components/jquery/dist/jquery.min.js",
            "~/Scripts/admin-lte/js/app.js",
            "~/Scripts/admin-lte/plugins/fastclick/fastclick.js",
            "~/Content/adminlte/bower_components/bootstrap/dist/js/bootstrap.min.js",
            "~/Content/adminlte/dist/js/adminlte.min.js",
            "~/Content/adminlte/bower_components/datatables.net/js/jquery.dataTables.min.js",
            "~/Content/adminlte/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
             "~/Scripts/DataTables/Buttons-1.5.1/js/dataTables.buttons.min.js",
            "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.bootstrap.min.js",
            "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.print.min.js",
            "~/Scripts/DataTables/JSZip-2.5.0/jszip.min.js",
            "~/Scripts/DataTables/pdfmake-0.1.36/pdfmake.min.js",
            "~/Scripts/DataTables/pdfmake-0.1.36/vfs_fonts.js",
            "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.html5.min.js",
            "~/Scripts/DataTables/Buttons-1.5.1/js/buttons.colVis.min.js"
            //"~/Scripts/jquery.table2excel.js"
            //"~/Content/Theme/adminlte/bower_components/select2/dist/js/select2.js",

            ));

            //BundleTable.EnableOptimizations = true;
            BundleTable.EnableOptimizations = false;
        }
    }
}
