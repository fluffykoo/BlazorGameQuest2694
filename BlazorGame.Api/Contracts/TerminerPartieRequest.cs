using System.ComponentModel.DataAnnotations;

namespace BlazorGame.Api.Contracts;

public class TerminerPartieRequest
{
    [Required]
    public int ScoreFinal { get; set; }
}
