using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystem.Enum
{
    public enum PositionEnum
    {
        PRESIDENT,
        [Display(Name = "VICE PRESIDENT")]
        VICEPRESIDENT,
        [Display(Name = "MINISTER OF FINANCE")]
        MINISTEROFFINANCE,
        [Display(Name = "MINISTER OF SPORTS")]
        MINISTEROFSPORTS,
        [Display(Name = "MINISTER OF ACADEMICS")]
        MINISTEROFACADEMICS
    }
}