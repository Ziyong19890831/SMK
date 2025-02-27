$(function () {
    //blockUI
    $.extend($.blockUI.defaults, {
        message: "<div class='spinner'></div>",
        baseZ: 9000,
        css: {
            padding: 0,
            margin: 0,
            width: '30%',
            top: '40%',
            left: '35%',
            textAlign: 'center',
            color: '#000',
            cursor: 'wait',
        },
    });

    function setIdleTimer() {
        if (!window.location.pathname.toLowerCase().startsWith('../home/login')) {
            window.setTimeout(function () {
                alert('您已30分鐘無動作，即將為您登出系統!');
                $.post('../Home/LogOff', {}, function (res) {
                    window.location.href = '../Home/Login';
                });
            }, 1800000);
        }
    }

    var timeoutHandle = setIdleTimer();

    //Ajax Init
    $(document).ajaxStart(function () {
        showMask();
    }).ajaxStop(hideMask);
    $.ajaxSetup({
        type: 'POST',
        method: 'POST',
        cache: false,
        headers: {
            "RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
        },
        complete: function () {
            window.clearTimeout(timeoutHandle);
            setIdleTimer();
        },
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest"); //強制加入XHR，以避免IsAjaxRequest誤判的狀況
        //},
        error: function (xhr, textStatus, errorThrown) {
            alert("請注意：" + textStatus + "-" + errorThrown);
        }
    });

    $('a').click(function () {
        window.clearTimeout(timeoutHandle);
        setIdleTimer();
    })

    $("*").submit(function () {
        window.clearTimeout(timeoutHandle);
        setIdleTimer();
    });

    //datepicker
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    $(".smkdate").datepicker({
        //maxDate: new Date(),
        changeYear: true,
        changeMonth: true
    });
    $(".smkdate3").datepicker({
        //maxDate: new Date(),
        changeYear: true,
        changeMonth: true,
        dateFormat: 'yy/mm'
    });

    $(".smkdate1").datepicker({
        changeMonth: true,
        changeYear: true,
        onClose: function (dateText, inst) {
            $(this).datepicker('setDate', inst.selectedYear + '/' + (inst.selectedMonth + 1) + '/' + '1');
            $('.ui-datepicker-calendar').css({ display: "block" })
        },
        onChangeMonthYear: function (year, month, inst) {
            $(inst).datepicker('setDate', new Date(year, month, 1));
            $('.ui-datepicker-calendar').css({ display: "block" })
        },
        beforeShow: function () {
            $('.ui-datepicker-calendar').toggle(() => {
                $('.ui-datepicker-calendar').css({ display: "none" })
            });
        },
    });

    $(".smkdate2").datepicker($.datepicker.regional['en'], {
        changeYear: true,
        changeMonth: true,
        dateFormat: 'yyyyMMdd'
    });

    $("#btnChangePincode").on("click", function (e) {

        if ($("#txtPwd").val() !== $("#txtPwdConfirm").val()) {
            alert("輸入密碼不一致，請檢查!");
            e.preventDefault();
            return;
        }
        if ($('#myForm')[0].checkValidity()) {
            $('#myForm').submit();
        } else {
            document.querySelector('#txtPwd').reportValidity()
        }
    });

    // Add a request interceptor
    axios.interceptors.request.use(function (config) {
        // Do something before request is sent
        config.validateStatus = function () {
            return true;
        }
        if (!config.hideMask) {
            showMask();
        }

        config.headers.RequestVerificationToken = $("input[name='__RequestVerificationToken']").val();

        return config;
    }, function (error) {
        // Do something with request error
        return Promise.reject(error);
    });

    // Add a response interceptor
    axios.interceptors.response.use(function (response) {
        // Any status code that lie within the range of 2xx cause this function to trigger
        // Do something with response data
        hideMask();

        var status = response.status;
        if (status < 200 || 300 <= status) {
            throw response;
        }

        if (response.config.method === 'post') {
            $('html, body').scrollTop(0);
        }

        return response;
    }, function (error) {
        // Any status codes that falls outside the range of 2xx cause this function to trigger
        // Do something with response error
        hideMask();
        return Promise.reject(error);
    });

    Vue.config.errorHandler = function (err, vm) {
        console.log(err);
        if (err) {
            if (err.status === 401 || err.status === 403) {
                myAlert.error("錯誤提示", "權限不足", true);
                $('html, body').scrollTop(0);
                return;
            }

            if (err.data && err.data.message) {
                myAlert.error("錯誤提示", err.data.message, true);
                $('html, body').scrollTop(0);
            }
        }
    };
});

/**alert**/

var myAlert = (function () {
    $(".alert .close")
        .on("click", function () {
            $(this).parent(".alert").hide();
        })
        .trigger("click");

    $("#sysMsgArea").removeAttr("style");

    var $success = $("#msgSuccessAlert");
    var $info = $("#msgInfoAlert");
    var $warning = $("#msgWarningAlert");
    var $error = $("#msgErrorAlert");


    function showAlert(el, title, msg, hideAll) {
        if (!title && !msg) return;
        if (hideAll) {
            $success.hide();
            $info.hide();
            $warning.hide();
            $error.hide();
        }
        $(".alert-title", el).text(title);
        $(".alert-msg", el).text(msg);
        el.show();
        setTimeout(function () { el.hide(); }, 60000);
    }

    return {
        success: function (title, msg, hideAll) {
            showAlert($success, title, msg, hideAll);
        },
        info: function (title, msg, hideAll) {
            showAlert($info, title, msg, hideAll);
        },
        warning: function (title, msg, hideAll) {
            showAlert($warning, title, msg, hideAll);
        },
        error: function (title, msg, hideAll) {
            showAlert($error, title, msg, hideAll);
        },
    };
})();


//刪除
$(".fn-delete").on("click", function (e) {
    if (!confirm("確定刪除？")) {
        e.stopPropagation();
        e.preventDefault();
    }
});



//Global JS Function
var spinnerDom = [
    "<div class='spinner-bouncebar'> <div class='rect1'></div> <div class='rect2'></div> <div class='rect3'></div> <div class='rect4'></div> <div class='rect5'></div> </div>",
    "<div class='spinner-circle'> <div class= 'double-bounce1'></div> <div class='double-bounce2'></div> </div>",
    "<div class='spinner-flipcube'></div>",
    "<div class='spinner-2-rotating-cube'> <div class='cube1'></div> <div class='cube2'></div> </div>",
    "<div class='spinner-folding-cube'> <div class='sk-cube1 sk-cube'></div> <div class='sk-cube2 sk-cube'></div> <div class='sk-cube4 sk-cube'></div> <div class='sk-cube3 sk-cube'></div> </div>",
];
function showMask(loadingIndex) {
    var randomNum = (!isNaN(loadingIndex) && loadingIndex >= 0 && loadingIndex < spinnerDom.length)
        ? randomNum = loadingIndex
        : Math.floor(Math.random() * spinnerDom.length);

    $.blockUI({
        message: spinnerDom[randomNum]
    });
}

function hideMask() {
    $.unblockUI();
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


jQuery.download = function (url, data, method) {
    if (url && data) {
        data = typeof data == 'string' ? data : jQuery.param(data);
        //split params into form inputs
        var inputs = '';
        jQuery.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        //send request
        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
            .appendTo('body').submit().remove();
    };
};


/**
 * 抓取DataTable的表頭資料並轉成CheckBox
 * 參數如下 :
 * tableId = Table的ID
 * RowOne = 是否要全部的列 (true,false)
 * Row = 從第幾列開始載入資料
 */
const grabTableColumns = (tableId, RowOne, RowNum, ShowOrHideid, InsertID) => {
    // 使用 jQuery 選擇哪一個Table
    var headerRow = $("#" + tableId + " thead tr:eq(1)");

    if (!RowOne)
        var findData = "th";
    else
        var findData = `th:gt(${RowNum})`;
    var checkboxContainer = $(`#${InsertID}`);
    checkboxContainer.empty();
    var AllLock = `
            <div class = "col-12">
                <input type="checkbox" id = "${InsertID}_AllClock" value = "全選" onclick = "AllCheckBoxChecked('${InsertID}','${InsertID}_AllClock')"/>
                <label for="${InsertID}_AllClock" class="ml-1 mr-4 pt-1">全選</label>
            </div>
        `;
    checkboxContainer.append(AllLock);
    // 抓取每個格子的Text
    headerRow.find(findData).each(function () {
        var columnHeader = $(this).text().trim();
        var data = `
            <div class = "col-3">
                <input type="checkbox" id = "${columnHeader}_${InsertID}" name = "SelectCheckBox" value = "${columnHeader}"/>
                <label for="${columnHeader}_${InsertID}" class="ml-1 mr-4 pt-1">${columnHeader}</label>
            </div>
        `;
        checkboxContainer.append(data);
    });
    ShowOrHideID(ShowOrHideid);
}
/**
 * 顯示或隱藏指定欄位
 * 帶入要隱藏區域的ID
 * ShowOrHideid = 指定ID名稱
 */
const ShowOrHideID = (ShowOrHideid) => {
    var IDRange = $(`#${ShowOrHideid}`);
    if (IDRange.css("display") == "none") {
        IDRange.show();
    } else {
        IDRange.hide();
    }
}

/**
 * 全選方法
 * 帶入要勾選區域的ID
 * ShowOrHideid = 指定ID名稱
 */
const AllCheckBoxChecked = (ShowOrHideid,checkBox) => {
    var InputCheckbox = $(`#${ShowOrHideid} input:checkbox`);
    var Input = $(`#${checkBox}`);
    if (Input.is(":checked")) {
        InputCheckbox.prop("checked", true);
    } else {
        InputCheckbox.prop("checked", false);
    }
}
