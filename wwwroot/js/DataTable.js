$(document).ready(function () {
    $('#DataTable').DataTable({
        destroy: true,
        paging: true,      
        searching: true,    
        ordering: false,   
        info: true,         
        lengthChange: true, 
        pageLength: 10,     
    });
});

