<%@ language="JSCRIPT" %>
<script language="JScript" type="text/jscript" runat="server">
    //
    // Generate RSS feed of the 100 most common words (except the 
    // single character ones). Repeat each word twice, include
    // 40 words in each article, choose them at random for each
    // article.
    //
    var words = ['the','of','and','to','in','is','you','that','it','he','was','for','on','are','as',
                'with','his','they','at','be','this','have','from','or','one','had','by','word',
                'but','not','what','all','were','we','when','your','can','said','there','use','an',
                'each','which','she','do','how','their','if','will','up','other','about','out',
                'many','then','them','these','so','some','her','would','make','like','him','into',
                'time','has','look','two','more','write','go','see','number','no','way','could',
                'people','my','than','first','water','been','call','who','oil','its','now','find',
                'long','down','day','did','get','come','made','may','part'];

    var article = "";
    var prevWord = "";
    for (i = 0; i < 40; i++)
    {
        var word = words[Math.floor(Math.random() * words.length)];
        while(word == prevWord)
        {
            word = words[Math.floor(Math.random() * words.length)];     // Keep trying till get new word
        }
        article += " " + word + " " + word;
        prevWord = word;
    }

    var randTitle = Math.floor(Math.random() * 1000000);                 // Randomize title for RSS readers
    Response.ContentType = "text/xml";
    Response.Write("<rss version=\"2.0\">\r\n<channel>\r\n<item>\r\n<title>20 random words " + 
                "from top 100 English words [" + randTitle + "]</title>\r\n");
    Response.Write("<description>" + article + "</description>\r\n");
    Response.Write("</item>\r\n</channel>\r\n</rss>\r\n");
    
</script>
