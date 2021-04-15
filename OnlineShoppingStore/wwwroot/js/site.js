// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Javascript for datatable
$(document).ready(function () {
    $('#dataTable').DataTable({
        responsive: true,
        dom: "<'row p-1'<'col-sm-4 pt-1'l><'col-sm-4 pt-1'f><'col-sm-4'B>>" + "<'row'<'col-sm-12'tr>>" + "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                extend: 'copy',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5] //Your Colume value those you want
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5] //Your Colume value those you want
                }
            },

            {
                text: 'PDF',
                extend: 'pdf',
                messageTop: [
                    {
                        alignment: 'center',
                        fontSize: 14,
                        text: $("#tittle").val(),
                    }
                ],
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5] //Your Colume value those you want
                },
                messageBottom: null,
                customize: function (doc) {
                    //Create a date string that we use in the footer. Format is dd-mm-yyyy
                    var now = new Date();
                    var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                    doc.content.splice(0, 1, {
                        text: [{
                            text: 'Date: ' + jsDate + '       ' + 'Create By: Admin@gmail.com',
                            bold: true,
                            fontSize: 12
                        }
                            //,{
                            //    text: 'Custom message',
                            //    bold: true,
                            //    fontSize: 12
                            //}
                        ],
                        margin: [0, 0, 0, 12],
                        alignment: 'center'
                    });
                    // Logo converted to base64
                    // var logo = getBase64FromImageUrl('https://datatables.net/media/images/logo.png');
                    // The above call should work, but not when called from codepen.io
                    // So we use a online converter and paste the string in.
                    // Done on http://codebeautify.org/image-to-base64-converter
                    // It's a LONG string scroll down to see the rest of the code !!!
                    //var logo = null;
                    // A documentation reference can be found at
                    // https://github.com/bpampuch/pdfmake#getting-started
                    // Set page margins [left,top,right,bottom] or [horizontal,vertical]
                    // or one number for equal spread
                    // It's important to create enough space at the top for a header !!!
                    doc.pageMargins = [70, 60, 60, 60];
                    // Set the font size fot the entire document
                    doc.defaultStyle.fontSize = 11;
                    // Set the fontsize for the table header
                    doc.styles.tableHeader.fontSize = 11;
                    // Create a header object with 3 columns
                    // Left side: Logo
                    // Middle: brandname
                    // Right side: A document title
                    doc['header'] = (function () {
                        return {
                            columns: [
                                //{
                                //    image: logo,
                                //    width: 24
                                //},
                                //{
                                //    alignment: '',
                                //    italics: true,
                                //    text: 'dataTables',
                                //    fontSize: 18,
                                //    margin: [10, 0]
                                //},
                                {
                                    alignment: 'center',
                                    fontSize: 16,
                                    text: 'Online Shopping Store'
                                }
                            ],
                            margin: 20
                        };
                    });
                    // Create a footer object with 2 columns
                    // Left side: report creation date
                    // Right side: current page and total pages
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                {
                                    alignment: 'center',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: 20
                        };
                    });
                    // Change dataTable layout (Table styling)
                    // To use predefined layouts uncomment the line below and comment the custom lines below
                    // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                    var objLayout = {};
                    objLayout['hLineWidth'] = function (i) { return .5; };
                    objLayout['vLineWidth'] = function (i) { return .5; };
                    objLayout['hLineColor'] = function (i) { return '#aaa'; };
                    objLayout['vLineColor'] = function (i) { return '#aaa'; };
                    objLayout['paddingLeft'] = function (i) { return 4; };
                    objLayout['paddingRight'] = function (i) { return 4; };
                    doc.content[0].layout = objLayout;

                }
            }
        ]
    });
});

$('#topheader .nav a').on('click', function () {
    $('#topheader .nav').addClass('active');
});