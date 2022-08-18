using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store_TechniaclTask.DAL.Enums
{
    public enum SharedPermissions
    {
        [Display(Name = "BrowsPolicy")]
        BrowsPolicy=1,
        [Display(Name = "CreatePolicy")]
        CreatePolicy,
        [Display(Name = "DeletePolicy")]
        DeletePolicy,
        [Display(Name = "EditPolicy")]
        EditPolicy,
        [Display(Name = "ChangePasswordPolicy")]
        ChangePasswordPolicy,
    }
    //public enum SharedAccountsReportsPermissions
    //{
    //    [Display(Name = "AccountStatement")]
    //    AccountStatement=1,
    //    [Display(Name = "SearchReceipts")]
    //    SearchReceipts,
    //    [Display(Name = "PaymentOrdersReport")]
    //    PaymentOrders,
    //}
    //public enum SharedReportsPermissions
    //{
    //    //[Display(Name = "AuthorityOffersReport")]
    //    //AuthorityOffersReport,
    //    //[Display(Name = "AuthoritySalesReportForPeriod")]
    //    //AuthoritySalesReportForPeriod,
    //    //[Display(Name = "CustomerSalesReportForPeriod")]
    //    //CustomerSalesReportForPeriod,    
    //    //[Display(Name = "AuthorityPowerReport")]
    //    //AuthorityPowerReport,
    //    //[Display(Name = "GovernatesReport")]
    //    //GovernatesReport,
    //    //[Display(Name = "TurnedOffCustommersReport")]
    //    //TurnedOffCustommersReport, 
    //    //[Display(Name = "SupervisorRepresentative_CardStatusReport")]
    //    //SupervisorRepresentative_CardStatusReport,
    //    //[Display(Name = "AvailableNetworksInAuthorityContract")]
    //    //AvailableNetworksInAuthorityContract,      
    //    //[Display(Name = "AuthorityReport")]
    //    //AuthorityReport, 
    //    //[Display(Name = "CurrentlyStoppedCustomerCardsReport")]
    //    //CurrentlyStoppedCustomerCardsReport,      
    //    //[Display(Name = "CardNumberStatusReport")]
    //    //CardNumberStatusReport, 
    //    //[Display(Name = "BannedAuthoritiesReport")]
    //    //BannedAuthoritiesReport,  
    //    //[Display(Name = "AuthoritiesContractEndWithinPeriodReport")]
    //    //AuthoritiesContractEndWithinPeriodReport,
    //    //[Display(Name = "ContractsReport")]
    //    //ContractsReport,
    //    //[Display(Name = "AuthoritiesHasNotBeenSoldToDuringperiodReport")]
    //    //AuthoritiesHasNotBeenSoldToDuringperiodReport,    
    //    //[Display(Name = "CustomerReport")]
    //    //CustomerReport,  
    //    //[Display(Name = "CardNumbersReport")]
    //    //CardNumbersReport,      
    //    //[Display(Name = "SuspendedCardsBeforePeriod")]
    //    //SuspendedCardsBeforePeriod,
    //    //[Display(Name = "CardsWitoutCustomersReport")]
    //    //CardsWitoutCustomersReport, 
    //    //[Display(Name = "CustomersBills")]
    //    //CustomersBills,  
    //    //[Display(Name = "AuthorityCommissionsTotals_Report")]
    //    //AuthorityCommissionsTotals_Report, 
    //    //[Display(Name = "TurnOffReason_Report")]
    //    //TurnOffReason_Report, 
    //    //[Display(Name = "AuthorityMonthlyStatement")]
    //    //AuthorityMonthlyStatement,
    //    //[Display(Name = "DebitCustomersReport")]
    //    //DebitCustomersReport,   
    //    //[Display(Name = "CashCollectionsReport")]
    //    //CashCollectionsReport, 
    //    //[Display(Name = "MonthlyPaymentCalendarReport")]
    //    //MonthlyPaymentCalendarReport,   
    //    //[Display(Name = "AuthoritiesDidNotPayInMonthReport")]
    //    //AuthoritiesDidNotPayInMonthReport,
    //    //[Display(Name = "AuthoritiesDidNotRecivedCommission")]
    //    //AuthoritiesDidNotRecivedCommission,  
    //    //[Display(Name = "CustomersOnPensionersReport")]
    //    //CustomersOnPensionersReport,
    //    //[Display(Name = "CardsThatHaveBeenChaneBaqats")]
    //    //CardsThatHaveBeenChaneBaqats,  
    //    //[Display(Name = "EndedAuthorityOffer")]
    //    //EndedAuthorityOffer,
    //}
}
