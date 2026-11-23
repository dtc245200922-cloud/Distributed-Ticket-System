namespace Ticket.Shared;

public enum BookingStatus { Pending, Confirmed, Rejected }

public sealed class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Venue { get; set; } = "";
    public DateTime StartsAtUtc { get; set; }
    public int TotalTickets { get; set; }
    public int AvailableTickets { get; set; }
}

public sealed class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
}

public sealed class Booking
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public int UserId { get; set; }
    public Customer User { get; set; } = null!;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAtUtc { get; set; }
}

public sealed record CreateBookingRequest(int UserId, int EventId);
public sealed record BookingMessage(int BookingId);
