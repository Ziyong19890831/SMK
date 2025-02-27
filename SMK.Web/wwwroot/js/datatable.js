
var jqDataTable;
$(function () {
    //DataTable Init
    $.extend($.fn.dataTable.defaults, {
        language: {
            emptyTable: "查無資料",
            processing: "處理中...",
            loadingRecords: "載入中...",
            lengthMenu: "顯示 _MENU_ 筆結果",
            zeroRecords: "沒有符合的結果",
            info: "顯示第 _START_ 至 _END_ 筆，共 _TOTAL_ 筆資料",
            infoEmpty: "顯示第 0 至 0 筆結果，共 0 筆",
            infoFiltered: "(從 _MAX_ 筆資料中過濾)",
            infoPostFix: "",
            search: "搜尋：",
            paginate: {
                first: "第一頁",
                previous: "上一頁",
                next: "下一頁",
                last: "最末頁"
            },
            aria: {
                sortAscending: "：升冪排列",
                sortDescending: "：降冪排列"
            }
        },
        order: [],   //預設不排序
    });

    jqDataTable = $('.jqDataTable').DataTable({
        responsive: true,
        destroy: true, //解決重新載入表格報錯問題：Cannot reinitialise DataTable
        searching: false
        //initComplete: function (settings, json) {
        //    $('.jqDataTable').unblock();
        //}
    });
});