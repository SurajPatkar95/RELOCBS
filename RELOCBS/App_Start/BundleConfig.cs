using System.Web;
using System.Web.Optimization;

namespace RELOCBS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        , "~/Scripts/mvcvalidationextensions.unobtrusive.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));


            #region Custom Js
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/gridmvc.js",
                      "~/Scripts/Plugins/loadmask/jquery.loadmask.js",
                      "~/Scripts/Plugins/jquery.slimscroll.js",
                      "~/Scripts/Plugins/jquery.metisMenu.js",
                      "~/Scripts/Plugins/jquery.nestable.js",
                      "~/Scripts/Plugins/sweetalert/sweetalert.min.js",
                      "~/Scripts/Plugins/toastr/toastr.min.js",
                      "~/Scripts/Plugins/inspinia.js",
                      "~/Scripts/Plugins/pace.min.js",
                      "~/Scripts/Plugins/url.min.js",
                      "~/Scripts/Plugins/jquery.form.js",
                      "~/Scripts/Plugins/jquery.cookie.js",
                      "~/Scripts/Plugins/jQuery.flashMessage.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/moment-timezone-with-data-2010-2020.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/Plugins/jquery.peity.min.js",
                      "~/Scripts/Plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Scripts/Plugins/jasny-bootstrap/js/jasny-bootstrap.min.js",
                      "~/Scripts/jquery.unobtrusive-ajax.min.js",
                      "~/Scripts/Plugins/webui-popover/jquery.webui-popover.min.js",
                      "~/Scripts/Plugins/select2/js/select2.full.js",
                      "~/Scripts/Plugins/icheck/icheck.js",
                      "~/Scripts/Plugins/autocomplete/jquery.autocomplete.min.js",
                      "~/Scripts/Plugins/dotdotdot/jquery.dotdotdot.js",
                      "~/Scripts/Plugins/bootstrap-editable/js/bootstrap-editable.min.js",
                      "~/Scripts/Plugins/jquery-steps/jquery.steps.min.js",
                      "~/Scripts/Plugins/jsPdf/jspdf.min.js",
                      "~/Scripts/Plugins/clockpicker/clockpicker.js",
                      "~/Scripts/Utility.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/UnitConverter.js",
                      "~/Scripts/HtmVal.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqunobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/TableSort").Include(
                        "~/Scripts/Plugins/tableSorter/jquery.tablesorter.js"
                        ));


            #endregion

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/site.css"
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/bootstrap-datetimepicker.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                     "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
                     "~/Scripts/dropzone/basic.css",
                     "~/Scripts/dropzone/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-slider").Include(
                     "~/Scripts/Plugins/bootstrap-slider/bootstrap-slider.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-slidercss").Include(
                     "~/Scripts/Plugins/bootstrap-slider/css/bootstrap-slider.css"));


            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                     "~/Scripts/Plugins/jstree/jstree.js"));

            bundles.Add(new StyleBundle("~/Content/jstreecss").Include(
                     "~/Scripts/Plugins/jstree/themes/default/style.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap-multiselect").Include(
                     "~/Scripts/Plugins/bootstrap-multiselect/dist/js/bootstrap-multiselect.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-multiselectcss").Include(
                     "~/Scripts/Plugins/bootstrap-multiselect/dist/css/bootstrap-multiselect.css"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap-select").Include(
         "~/Scripts/bootstrap-select.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-selectcss").Include(
                     "~/Content/bootstrap-select.css"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap-toggle").Include(
                     "~/Scripts/Plugins/bootstrap-toggle/js/bootstrap-toggle.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-togglecss").Include(
                     "~/Scripts/Plugins/bootstrap-toggle/css/bootstrap-toggle.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                     "~/Scripts/Plugins/jquery-ui/jquery-ui.js",
                     "~/Scripts/Plugins/jquery-ui/jquery-ui-i18n.min.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                     "~/Scripts/Plugins/jquery-ui/jquery-ui.css"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/summernote").Include(
                     "~/Scripts/Plugins/summernote/summernote.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/star-rating").Include(
                     "~/Scripts/Plugins/bootstrap-star-rating/js/star-rating.js"));

            bundles.Add(new StyleBundle("~/Content/star-ratingcss").Include(
                     "~/Scripts/Plugins/bootstrap-star-rating/css/star-rating.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-fileinputcss").Include(
                     "~/Scripts/Plugins/bootstrap-fileinput/css/fileinput.css"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-fileinput").Include(
                     "~/Scripts/Plugins/bootstrap-fileinput/js/canvas-to-blob.js",
                     "~/Scripts/Plugins/bootstrap-fileinput/js/sortable.js",
                     "~/Scripts/Plugins/bootstrap-fileinput/js/purify.js",
                     "~/Scripts/Plugins/bootstrap-fileinput/js/fileinput.js",
                     "~/Scripts/Plugins/bootstrap-fileinput/themes/fa.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/summernote").Include(
                     "~/Scripts/Plugins/summernote/summernote.css"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/bgiframe").Include(
                     "~/Scripts/jquery.bgiframe.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/customwidgets").Include(
                     "~/Scripts/Plugins/GridCustomFilters/customwidgets.js"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Scripts/Plugins/loadmask/jquery.loadmask.css",
                      "~/Content/Gridmvc.css",
                      "~/Content/PagedList.css",
                      "~/Content/animate.css",
                      "~/Scripts/Plugins/jasny-bootstrap/css/jasny-bootstrap.css",
                      "~/Scripts/Plugins/sweetalert/sweetalert.css",
                      "~/Scripts/Plugins/toastr/toastr.min.css",
                      "~/Scripts/Plugins/webui-popover/jquery.webui-popover.css",
                      "~/Scripts/Plugins/select2/css/select2.css",
                      "~/Scripts/Plugins/icheck/skins/square/orange.css",
                      "~/Scripts/Plugins/bootstrap-editable/css/bootstrap-editable.css",
                      "~/Scripts/Plugins/jquery-steps/jquery.steps.css",
                      "~/Scripts/Plugins/clockpicker/clockpicker.css",
                      "~/Content/plugins/timeline.css",
                      "~/Content/style.css",
                      "~/Content/custom.css",
                      "~/Content/tablesort.css",
                      "~/Content/tableScrollbar.css"
                      ));

			bundles.Add(new StyleBundle("~/Content/CustomLogin").Include(
					  "~/Scripts/Plugins/sweetalert/sweetalert.css",
					  "~/Scripts/Plugins/toastr/toastr.min.css",
					  "~/Content/Login.css"
				));
			bundles.Add(new ScriptBundle("~/bundles/ComboFill").Include(
					  "~/Scripts/FillCombo.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/SubCostHead").Include(
					  "~/Scripts/SubCostHeads.js"
				));

			bundles.Add(new ScriptBundle("~/bundles/MoveManage").Include(
					  "~/Scripts/MoveManage/MoveManage.js"
				));

		}
    }
}
