using System.Collections.Generic;

namespace hateoas.starter.Models
{
    public record Link(string Href, string Rel, string Method);
}