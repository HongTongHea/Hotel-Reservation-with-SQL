$(document).ready(function () {
    $('#DataTable').DataTable({
        paging: true,      
        searching: true,    
        ordering: false,   
        info: true,         
        lengthChange: true, 
        pageLength: 10,     
    });
});

