using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class FavoriteRequest
    {
        [Required] public int PropertyId { get; set; }

        [Required] public bool IsFavorite { get; set; }
    }
}