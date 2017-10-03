using BibleReader.model;
using BibleReader.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.grammar {
    public class Syllabifier {
        public List<Syllable> Syllabify(Letter[] word) {
            word = word.Where(x => !x.IsSeparator).ToArray();
            List<Syllable> syllables = new List<Syllable>();
            List<Letter> letters = new List<Letter>();
            if (word.Last().CouldBeFurtivePatach)       // TODO: Move to the utils code???
                word.Last().IsFurtivePatach = true;

            for (int ii = 0; ii < word.Length; ii++) {
                Letter letter = word[ii];
                Letter nextLetter = ii >= word.Length - 1 ? null : word[ii + 1];
                Letter nextNextLetter = ii >= word.Length - 2 ? null : word[ii + 2];

                letters.Add(letter);

                switch (letters.Count) {
                    case 1:
                        if (letter.TheLetter == HLetter.Vav && letter.Has(HAnnotation.PointDageshOrMapiq))
                            ComposeSyllable(syllables, letters, SyllableType.HolemVav);
                        else if (nextLetter == null)     // This is the last letter of the word
                            ComposeSyllable(syllables, letters, SyllableType.Open);
                        else if (nextLetter.HasVowel == LetterHasVowel.Yes && !nextLetter.CouldBeMater ||    // Next letter has vowel (but not cholem vav), so this is an Open syllable
                                 nextNextLetter != null && nextNextLetter.CouldBeMater)    // Next letter is followed by Mater, so this is an Open syllable
                            ComposeSyllable(syllables, letters, SyllableType.Open);
                        break;
                    case 2:         // Second letter
                        if (letter.CouldBeMater) {
                            letter.IsMater = true;
                            if (nextLetter == null || nextLetter.HasVowel == LetterHasVowel.Yes && !nextLetter.IsFurtivePatach)
                                ComposeSyllable(syllables, letters, SyllableType.Open);
                        } else
                            ComposeSyllable(syllables, letters, SyllableType.Closed);
                        break;
                    case 3:         // Third letter
                        ComposeSyllable(syllables, letters, SyllableType.Closed);
                        break;
                }
            }

            //if (letters.Count > 0)
            //    ComposeSyllable(syllables, letters, SyllableType.Closed);

            return syllables;
        }

        private void ComposeSyllable(List<Syllable> syllables, List<Letter> letters, SyllableType type) {
            Syllable syllable = new Syllable(letters.ToArray(), type);
            syllables.Add(syllable);
            letters.Clear();
        }
    }
}
