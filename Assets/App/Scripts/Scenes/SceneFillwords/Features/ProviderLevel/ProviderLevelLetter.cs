namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderLevelLetter
    {
        public char letter { get; private set; }
        public int index { get; private set; }

        public ProviderLevelLetter(char letter, int index)
        {
            this.letter = letter;
            this.index = index;
        }
    }
}

