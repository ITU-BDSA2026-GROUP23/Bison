namespace Bison.Core.DTOs;

public record ObservationDTO(
    string Author,
    string Text,
    string Timestamp,
    byte[]? ImageData,
    string? ImageContentType,
    int PostId
    
);

public record CommentDTO(
    string Author,
    string Text,
    string Timestamp
);

public record ProposalDTO(
    string Author,
    string Text,
    string Timestamp,
    string TaxonName
);

public record TaxonDTO(
    string TaxonID,
    string DanishVernacularName
);

public record AuthorDTO(
    string Name,
    string Email
);