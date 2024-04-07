using System.Text;

namespace InteEnSingelGenerator;

public class StringList : List<string>
{
    public string SpeakList()
    {
        switch (Count)
        {
            case 1:
                return this[0];
            case 2:
                return $"{this[0]} och {this[1]}";
            default:
                if (Count > 0)
                {
                    var s = new StringBuilder();

                    for (var i = 0; i < Count; i++)
                    {
                        if (i == 0)
                            s.Append(this[i]);
                        else if (i > 0 && i < Count - 1)
                            s.Append($", {this[i]}");
                        else
                            s.Append($" och {this[i]}");
                    }

                    return s.ToString();
                }

                return "";
        }
    }

    public string CommaList() =>
        string.Join( ", ", this);
}