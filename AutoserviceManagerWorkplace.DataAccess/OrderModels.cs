//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoserviceManagerWorkplace.UI.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderModels
    {
        public int OrderId { get; set; }
        public string OrderContent { get; set; }
        public System.DateTime StartDateOfWork { get; set; }
        public Nullable<System.DateTime> EndDateOfWork { get; set; }
        public int Price { get; set; }
        public Nullable<int> Car_CarId { get; set; }
    
        public virtual CarModels CarModels { get; set; }
    }
}