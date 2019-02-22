/***

* This script provides the facility to build own custom filter widgets:

* 1. Specify widget type for column:
*       columns.Add(o => o => o.AdditionalServiceName)
*       .SetFilterWidgetType("CustomAdditionalServicesFilterWidget")

* 2. Register script with custom widget on the page:
*       <script src="@Url.Content("~/Scripts/gridmvc.customwidgets.js")" type="text/javascript"></script>

* 3. Register your own widget in Grid.Mvc:
*       GridMvc.addFilterWidget(new CustomAddSrvFilterWidget());

*/

/***
* CustomAddSrvFilterWidget - Provides filter for additional service name column in this project
* This widget onRenders select list with avaliable customers.
*/

function CustomAddSrvFilterWidget() {

    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {

        return ["CustomAdditionalServicesFilterWidget"];
    };

    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };

    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang      - current language settings;
    * typeName  - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * values    - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb        - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data      - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(); //onRender filter widget
        this.loadAdditionalServices(); //load customer's list from the server
        this.registerEvents(); //handle events
    };

    this.renderWidget = function () {
        var html = '<p><i>This is custom filter widget.</i></p>\
                    <p>Select additionalserviceslist to filter:</p>\
                    <select style="width:250px;" class="grid-filter-type additionalserviceslist form-control">\
                    </select>';
        this.container.append(html);
    };

    /***
    * Method loads all Additional Services from the server via Ajax:
    */
    this.loadAdditionalServices = function () {
        var $this = this;
        $.post("/Move/GetCustomersNames", function (data) {
            $this.fillAdditionalServices(data);
        });
    };

    /***
    * Method fill AdditionalServices select list by data
    */
    this.fillAdditionalServices = function (items) {
        var additionalservicesList = this.container.find(".additionalserviceslist");
        for (var i = 0; i < items.length; i++) {
            additionalservicesList.append('<option ' + (items[i].ItemName == this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i].ItemName + '">' + items[i].ItemName + '</option>');
        }
    };

    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {
        //get list with customers
        var additionalservicesList = this.container.find(".additionalserviceslist");

        //save current context:
        var $context = this;

        //register onclick event handler
        additionalservicesList.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });

    };

}