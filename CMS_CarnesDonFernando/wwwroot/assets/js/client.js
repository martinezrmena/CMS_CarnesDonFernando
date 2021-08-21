/**
 * @author Kevin Sandoval <ksandoval@cgclatam.com>
 * Funciones inicializadoras
 */

$(document).ready(function() {

    /**
     * grid dashboard
     */
    grid();

    /**
     * new datepicker
     */

    $('[data-toggle="datepicker"]').datepicker({
        "language": "es-ES",
        "format": "dd/mm/yyyy"
    });

    /**
     * new datatable
     */
    initDataTable();
    $('.datatable').dataTable();

    /**
     * new button and added to the table
     */
    var btn = newBtnModal();
    var trigger = setTimeout(function () {
        triggerTb(btn);
    }, 1500);

    /**
     * invert color nav
     */
    $(window).scroll(navbar);
    /**
     * resize grid
     */
    $(window).resize(grid);

    setInterval(function () {
        var heightTable = $(".row-floating").height();
        $(".banner-footer").height(heightTable);
    }, 5); 

});

function triggerTb(btn) {
    //if ($('.dataTables_wrapper').length !== 0) {
    if ($('.dataTables_wrapper') !== 0) {
        $('.dataTables_wrapper div.row:first > div:first').html(btn);
        $('.dataTables_wrapper div.row:first').css("cssText", "padding-bottom: 30px !important;");
        $('.dataTables_wrapper div.row:first > div:first').css("cssText", "text-align: left;");
        $('.dataTables_filter label').html("<div class='input-group'>  <input type='search' class='form-control form-control-sm' aria-controls='DataTables_Table_0'>   <div class='input-group-append'><span class='input-group-text'><i class='c-lightgrey fas fa-search'></i></span></div>   </div>");
    }
}

function newBtnModal() {
    var btn = document.createElement('button');
    btn.className = 'btn btn-primary btn-red';
    btn.innerHTML = 'Nuevo';
    btn.setAttribute('data-toggle', 'modal');
    btn.setAttribute('data-target', '#modal-register');
    btn.setAttribute('id', 'btnNuevo');
    btn.setAttribute('onclick', 'CargarFormulario(null, null)');
    return btn;
}

function navbar() {
    if ($(".navbar").length != 0) {
        if ($(".navbar").offset().top > 10) {
            $(".navbar").addClass("bg-inverse");
        } else {
            $(".navbar").removeClass("bg-inverse");
        }
    }
}

function grid() {
    if ($('.grid-div-main').length != 0) {
        var docHeight = $(window).height();
        var divBottom = docHeight / 16;
        var box = $('.grid-div-main a').height();
        docHeight = docHeight - divBottom;

        $('.grid-div-main').css('height', $(window).height());
        $('.grid-div-main').css('padding-bottom', divBottom);
        $('.grid-div-main').css('padding-top', docHeight - box);
    }
}

function initDataTable() {
    $.extend($.fn.dataTable.defaults, {
        "destroy": true,
        "responsive": {
            details: {
                renderer: $.fn.dataTable.Responsive.renderer.tableAll({
                    tableClass: 'table'
                })
            }
        },
        "pageLength": 5,
        "bLengthChange": false,
        "language": {
            'url': 'https://cdn.datatables.net/plug-ins/a5734b29083/i18n/Spanish.json'
        }
    });
}

