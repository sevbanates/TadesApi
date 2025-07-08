using System.ComponentModel.DataAnnotations;

namespace TadesApi.Models.ActionsEnum;

public enum UsersSecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}

public enum ControllerActionRoleSecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}

public enum ClientSecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
    [Display(Name = "Manage")] Manage = 16,
}


public enum LibrarySecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}

public enum ScheduleEventSecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}


public enum MessageSecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}

public enum InquirySecurity
{
    [Display(Name = "List")] List = 1,

    [Display(Name = "View")] View = 2,

    [Display(Name = "Save")] Save = 4,

    [Display(Name = "Delete")] Delete = 8,
}