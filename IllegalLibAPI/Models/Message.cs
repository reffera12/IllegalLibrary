using MimeKit;

public class Message{
    public MailboxAddress To { get; set;}
    public string Subject { get; set;}
    public string Content { get; set;}

    public Message(MailboxAddress to, string subject, string content)
    {
        To = to;
        Subject = subject;
        Content = content;
    }
}