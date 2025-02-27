(function (factory) {
    if (typeof define === "function" && define.amd) {
        // AMD. Register as an anonymous module.
        define(["../widgets/datepicker"], factory);
    } else {

        // Browser globals
        factory(jQuery.datepicker);
    }
}(function (datepicker) {
   
    datepicker.regional["zh-TW"] = {
        closeText: "關閉",
        prevText: "&#x3C;上個月",
        nextText: "下個月&#x3E;",
        currentText: "今天",
        monthNames: ["一月", "二月", "三月", "四月", "五月", "六月",
            "七月", "八月", "九月", "十月", "十一月", "十二月"],
        monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月",
            "七月", "八月", "九月", "十月", "十一月", "十二月"],
        dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
        dayNamesShort: ["週日", "週一", "週二", "週三", "週四", "週五", "週六"],
        dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
        weekHeader: "週",
        dateFormat: "yy/mm/dd",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: true,
        yearSuffix: "年",
        onSelect: function (dateText, inst) {
   
                var dateFormate = inst.settings.dateFormat == null ? "yy/mm/dd" : inst.settings.dateFormat; //取出格式文字
                var reM = /m+/g;
                var reD = /d+/g;
                var objDate = {

                    y: inst.selectedYear - 1911 < 0 ? inst.selectedYear : padLeft((inst.selectedYear - 1911).toString(), 3, "0"),
                    m: padLeft(String(inst.selectedMonth + 1), 2, "0"),
                    d: padLeft(String(inst.selectedDay), 2, "0")
                };
                $.each(objDate, function (k, v) {
                    var re = new RegExp(k + "+");
                    dateFormate = dateFormate.replace(re, v);
                });
                inst.input.val(dateFormate);
            
            
        }
    };
    datepicker.setDefaults(datepicker.regional["zh-TW"]);

    return datepicker.regional["zh-TW"];

}));

var old_generateMonthYearHeader = $.datepicker._generateMonthYearHeader;
var old_get = $.datepicker._get;
var old_CloseFn = $.datepicker._updateDatepicker;
$.extend($.datepicker, {

    /* Parse existing date and initialise date picker. */

    _setDateFromField: function (inst, noDefault) {
        if (inst.input.val() == inst.lastVal) {
            return;
        }
        
        var dateFormat = this._get(inst, 'dateFormat');
        var dates = inst.lastVal = inst.input ? inst.input.val() : null;
        var date, defaultDate;
        date = defaultDate = this._getDefaultDate(inst);
        if (dates.length == 6) {
            dates += "/01";
        }
        var settings = this._getFormatConfig(inst);
        try {
            //date = this.parseDate(dateFormat, dates, settings) || defaultDate;
            if (dates.length > 0) {
                var dateArr = dates.split("/");
                var year = parseInt(dateArr[0], 10) + 1911;
                var month = parseInt(dateArr[1], 10) - 1;
                var day = parseInt(dateArr[2], 10);
                date = new Date(year, month, day);
            }
        } catch (event) {
            this.log(event);
            dates = (noDefault ? '' : dates);
        }
        inst.selectedDay = date.getDate();
        inst.drawMonth = inst.selectedMonth = date.getMonth();
        inst.drawYear = date.getFullYear();
        inst.selectedYear = date.getFullYear();
        inst.currentDay = (dates ? date.getDate() : 0);
        inst.currentMonth = (dates ? date.getMonth() : 0);
        inst.currentYear = (dates ? date.getFullYear() : 0);
        this._adjustInstDate(inst);

    },

    _daylightSavingAdjust: function (date) {
        if (!date) return null;
        date.setHours(date.getHours() > 12 ? date.getHours() + 2 : 0);
        if (!date) return null;
        if ((date.getFullYear() - 1911) > 0)
            date.getFullYear((date.getFullYear() - 1911));
        else
            date.getFullYear((date.getFullYear()));
        return date;

    },

    _taiwanDateAdjust: function (date) {
        if (!date) return null;
        if ((date.getFullYear() - 1911) > 0)
            date.setFullYear((date.getFullYear() - 1911), date.getMonth(), date.getDay());
        else
            date.setFullYear((date.getFullYear()), date.getMonth(), date.getDay());
        return date;

    },

    /* Generate the month and year header. */

    _generateMonthYearHeader: function (inst, drawMonth, drawYear, minDate, maxDate, secondary, monthNames, monthNamesShort) {
        var htmlYearMonth = old_generateMonthYearHeader.apply(this, [inst, drawMonth, drawYear, minDate, maxDate, secondary, monthNames, monthNamesShort]);
        if ($(htmlYearMonth).find(".ui-datepicker-year").length > 0) {
            htmlYearMonth = $(htmlYearMonth).find(".ui-datepicker-year").find("option").each(function (i, e) {
                if (Number(e.value) - 1911 > 0) $(e).text((Number(e.innerText) - 1911) + "年");
            }).end().end().get(0).outerHTML;
        }
        return htmlYearMonth;
    },
    _get: function (a, b) {
        a.selectedYear = a.selectedYear - 1911 < 0 ? a.selectedYear + 1911 : a.selectedYear;
        a.drawYear = a.drawYear - 1911 < 0 ? a.drawYear + 1911 : a.drawYear;
        a.curreatYear = a.curreatYear - 1911 < 0 ? a.curreatYear + 1911 : a.curreatYear;
        return old_get.apply(this, [a, b]);
    },
    _updateDatepicker: function (inst) {
        old_CloseFn.call(this, inst);
        $(this).datepicker("widget").find(".ui-datepicker-buttonpane").children(":last")
            .click(function (e) {
                inst.input.val("");
            });
    },
    _setDateDatepicker: function (a, b) {
        if (a = this._getInst(a)) { this._setDate(a, b); this._updateDatepicker(a); this._updateAlternate(a) }
    },
    _widgetDatepicker: function () {
        return this.dpDiv
    },
    //_formatDate: function (inst, day, month, year) {
    //    if (!day) {
    //        inst.currentDay = inst.selectedDay;
    //        inst.currentMonth = inst.selectedMonth;
    //        inst.currentYear = inst.selectedYear;
    //    }
    //    var date = (day ? (typeof day == 'object' ? day :
    //        this._daylightSavingAdjust(new Date(year, month, day))) :
    //        this._daylightSavingAdjust(new Date(inst.currentYear, inst.currentMonth, inst.currentDay)));
    //    return (date.getFullYear() - 1911 < 100 ? "0" + (date.getFullYear() - 1911) : (date.getFullYear() - 1911)) + "/" +
    //        (date.getMonth() < 9 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1)) + "/" +
    //        (date.getDate() < 10 ? "0" + date.getDate() : date.getDate());
    //}
});
function padLeft(str, length, sign) {
    if (str.length >= length) return str;
    else return padLeft(sign + str, length, sign);
}