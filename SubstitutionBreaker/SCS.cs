﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubstitutionBreaker
{
    public partial class SCS : Form
    {
        private Data text = null;
        private NGramData cipherTextData = null;
        private LangData engData = null;
        int shift_Index = 0;
        int runs = 0;

        double[] wieghts = new double[] { .7, .8, .9, 1, .9, .8, .7 };

        public SCS()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            engData = new LangData();
        }

        //Open File

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the openToolStripMenuItem control.
        ///   </para>
        ///   <para>
        ///   By opening a file to read the cipher out of and prepare it for later processing.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        text = new Data(reader.ReadToEnd());
                    }
                }
            }
            if (text.cipherText == "")
            {
                MessageBox.Show("File Invalid", "File has no aphla character", MessageBoxButtons.OK);
                text = null;
            }
            else
            {
                shift_Index = 0;
                runs = 0;
                cipherTextData = new NGramData(text.cipherText);
                CheckData();

                SetTBs();
                C1.Text = MakePrintable(text.cipherText);
                C2.Text = MakePrintable(text.cipherText);
                C3.Text = MakePrintable(text.cipherText);
                C4.Text = MakePrintable(text.cipherText);
                P1.Text = MakePrintable(text.Plaintext);
                P2.Text = MakePrintable(text.Plaintext);
                P3.Text = MakePrintable(text.Plaintext);
                P4.Text = MakePrintable(text.Plaintext);

                AffCipherText1.Items.Clear();
                AffCipherText2.Items.Clear();
                for (int i = 0; i < cipherTextData.uGramArray.Length; i++)
                {
                    AffCipherText1.Items.Add(cipherTextData.uGramArray[cipherTextData.uGramArray.Length - i - 1]);
                    AffCipherText2.Items.Add(cipherTextData.uGramArray[cipherTextData.uGramArray.Length - i - 1]);
                }

                AffPlainText1.Items.Clear();
                AffPlainText2.Items.Clear();
                for (int i = 0; i < engData.charFreq.Length; i++)
                {
                    AffPlainText1.Items.Add(engData.charFreq[engData.charFreq.Length - i - 1]);
                    AffPlainText2.Items.Add(engData.charFreq[engData.charFreq.Length - i - 1]);
                }

                HillCipherText1.Items.Clear();
                HillCipherText2.Items.Clear();
                for (int i = 0; i < cipherTextData.bGramArrayNonRepeat.Length; i++)
                {
                    HillCipherText1.Items.Add(cipherTextData.bGramArrayNonRepeat[cipherTextData.bGramArrayNonRepeat.Length - i - 1]);
                    HillCipherText2.Items.Add(cipherTextData.bGramArrayNonRepeat[cipherTextData.bGramArrayNonRepeat.Length - i - 1]);
                }

                HillPlainText1.Items.Clear();
                HillPlainText2.Items.Clear();
                for (int i = 0; i < engData.bigramMostCommonData.Length; i++)
                {
                    HillPlainText1.Items.Add(engData.bigramMostCommonData[engData.bigramMostCommonData.Length - i - 1]);
                    HillPlainText2.Items.Add(engData.bigramMostCommonData[engData.bigramMostCommonData.Length - i - 1]);
                }

                K2.Text = text.getAssign();
                K1.Text = "Shift:  ...";
            }
        }

        /// <summary>
        /// Checks whether a letter exist in the cipher text data. If the 
        /// letter does not exist assign its plain text geuss as 0 so it wont
        /// be used later.
        /// </summary>
        private void CheckData()
        {
            if (cipherTextData.uGramArray.Length < 26)
            {
                int i = 0, diff = 26 - cipherTextData.uGramArray.Length;

                for (int j = 0; j < 26 && i < diff; j++)
                {
                    if (!Exists(j))
                    {
                        text.assign[j] = "0";
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a letter exists in the ciphertext.
        /// </summary>
        /// <param name="j"> 
        /// Int representation of the letter to check (A-Z→0-25) .
        /// </param>
        /// <returns>
        /// True if the letter was found in the ciphertext, false otherwise.
        /// </returns>
        private bool Exists(int j)
        {
            bool found = false;
            foreach (var item in cipherTextData.uGramArray)
            {
                found = item.Item1 == ((char) (j + 'A')).ToString();
                if (found)
                {
                    return found;
                }
            }
            return found;
        }

        /// <summary>
        /// Sets the substitution cipher assignment TextBox's to a default values 
        /// ('0' if not in the cipher text, '-' otherwise).
        /// </summary>
        private void SetTBs()
        {
            int i = 0;
            A.Text = text.assign[i++];
            B.Text = text.assign[i++];
            C.Text = text.assign[i++];
            D.Text = text.assign[i++];
            E.Text = text.assign[i++];
            F.Text = text.assign[i++];
            G.Text = text.assign[i++];
            H.Text = text.assign[i++];
            I.Text = text.assign[i++];
            J.Text = text.assign[i++];
            K.Text = text.assign[i++];
            L.Text = text.assign[i++];
            M.Text = text.assign[i++];
            N.Text = text.assign[i++];
            O.Text = text.assign[i++];
            P.Text = text.assign[i++];
            Q.Text = text.assign[i++];
            R.Text = text.assign[i++];
            S.Text = text.assign[i++];
            T.Text = text.assign[i++];
            U.Text = text.assign[i++];
            V.Text = text.assign[i++];
            W.Text = text.assign[i++];
            X.Text = text.assign[i++];
            Y.Text = text.assign[i++];
            Z.Text = text.assign[i++];
        }

        /// <summary>
        /// Converts the plaintext or ciphertext into blocks of 5 characters
        /// and 8 blocks per line for display so the user can better see 
        /// changes as they use the substitution solver.
        /// </summary>
        /// <param name="Text">
        /// The text to be formatted.
        /// </param>
        /// <returns>
        /// The formatted text.
        /// </returns>
        private string MakePrintable(string Text)
        {
            StringBuilder res = new StringBuilder(Text.Length * 2);
            int i = 0;

            for (; i + 5 < Text.Length;)
            {
                res.Append(Text.Substring(i, 5) + " ");
                if ((i = i + 5) % 8 == 0)
                {
                    res.Append("\n");
                }
            }
            res.Append(Text.Substring(i) + "\n");

            return res.ToString();
        }
        //End Open File

        //Reset


        /// <summary>
        ///   <para>
        ///   Handles the Click event of the resetToolStripMenuItem control.
        ///   </para>
        ///   <para>
        ///   Resets the cipher so you can retry
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text != null)
            {
                text = new Data(text.cipherText);
                P1.Text = text.Plaintext;
                shift_Index = 0;
                runs = 0;
                K1.Text = shift_Index.ToString();

                SetTBs();
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }
        //End Reset

        //Save a report


        /// <summary>
        ///   <para>
        ///   Handles the Click event of the printReportToolStripMenuItem control.
        ///   </para>
        ///   <para>
        ///   Creates a report file 
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void printReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text != null)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "Report.txt";
                save.Filter = "Text File | *.txt";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(save.OpenFile());

                    string report = text.ToString();
                    int cur = 0, next = report.IndexOf('$');

                    while (next != -1)
                    {
                        writer.WriteLine(report.Substring(cur, next - cur));
                        cur = next + 1;
                        next = report.IndexOf('$', cur);
                    }

                    writer.WriteLine(report.Substring(cur));

                    writer.Dispose();
                }
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }
        //End Report

        //Shift
        private void ShiftGuess_Click(object sender, EventArgs e)
        {
            if (text != null)
            {
                int UGramLen = cipherTextData.uGramArray.Length-1;

                if (shift_Index >= UGramLen)
                    shift_Index = 0;

                int shift =  cipherTextData.uGramArray[UGramLen-(shift_Index++)].Item1[0]-'E';
                K1.Text = "Shift: " + shift;
                text.setShift(shift++);
                P1.Text = MakePrintable(text.Plaintext);
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }
        //End Shift

        //Sub

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the SubGuess control.
        ///   </para>
        ///   <para>
        ///   Makes a guess when button is pressed.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void SubGuess_Click(object sender, EventArgs e)
        {
            if (text != null)
            {
                MakeGuess(runs++);
                P2.Text = MakePrintable(text.Plaintext);
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }

        /// <summary>  
        /// Generates geuss weights for what it thinks is the most probable 
        /// plaintext character assigned to a ciphertext character.
        /// </summary>
        /// <param name="runs">
        /// The number of times this function has run.
        /// </param>
        private void MakeGuess(int runs)
        {
            double[,] guessWeight = new double[26, 26];

            UnigramAnalisis(guessWeight);
            if (runs > 0)
            {
                BigramAnalisis(guessWeight);
                TrigramAnalisis(guessWeight);
            }

            printArray(guessWeight);
        }

        /// <summary>
        /// Prints the top three guess weight, plaintext character pairs for
        /// each cipher text character yet to be assigned.
        /// </summary>
        /// <param name="guessWeight">
        /// The guess weight array.
        /// </param>
        private void printArray(double[,] guessWeight)
        {
            StringBuilder res = new StringBuilder(26 * 61);
            List<Tuple<int, int, double>> topPick;

            res.Append("\n");

            for (int x = 0; x < 26; x++)
            {
                topPick = GetTopValuesInRow(guessWeight, x);

                res.AppendFormat("{0,4:s}  | ", ((char) (x + 'A')).ToString());

                for (int k = 0; k < 3 && k < topPick.Count; k++)
                {
                    res.AppendFormat("{0,4:s} ", ((char) (topPick[k].Item2 + 'A')).ToString());
                    res.AppendFormat("{0,8:f6} ", topPick[k].Item3);
                    res.Append(" | ");
                }


                res.Append("\n");
            }

            res.Append("\n");

            K2.Text = text.getAssign();
            K2.Text += res.ToString();
        }

        /// <summary>
        /// Gets the top values in row by preforming an out of place insertion
        /// sort on the row.
        /// </summary>
        /// <param name="guessWeight">
        /// The guess weight array.
        /// </param>
        /// <param name="x">
        /// The row int the array to sort.
        /// </param>
        /// <returns>
        /// A sorted list of assignments and their wieghts
        /// </returns>
        private List<Tuple<int, int, double>> GetTopValuesInRow(double[,] guessWeight, int x)
        {
            List<Tuple<int, int, double>> topPick = new List<Tuple<int, int, double>>();

            int y;
            double w;
            bool found;

            if (text.assign[x] == "-")
            {
                for (y = 0; y < 26; y++)
                {
                    if (NotInArray(y))
                    {
                        if (!topPick.Any())
                        {
                            topPick.Add(new Tuple<int, int, double>(x, y, guessWeight[x, y]));
                        }
                        else
                        {
                            found = false;
                            w = guessWeight[x, y];

                            for (int k = 0; k < topPick.Count; k++)
                            {
                                if (!topPick.Contains(new Tuple<int, int, double>(x, y, w)))
                                {
                                    if (w > topPick[k].Item3)
                                    {
                                        topPick.Insert(k, new Tuple<int, int, double>(x, y, w));
                                        found = true;
                                        break;
                                    }
                                }
                            }
                            if (!found)
                            {
                                topPick.Add(new Tuple<int, int, double>(x, y, w));
                            }
                        }
                    }
                }
            }
            return topPick;
        }

        /// <summary>  
        /// Checks whether a ciphertext character has already been assigned 
        /// to a plaintext guess.
        /// </summary>
        /// <param name="j">
        /// The int representation of the character (A-Z→0-25).
        /// </param>
        /// <returns>
        /// True if the character has not already been assigned false otherwise.
        /// </returns>
        private bool NotInArray(int j)
        {
            foreach (var item in text.assign)
            {
                if (item == ((char) (j + 'A')).ToString())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// Modifies the guess weight array based on unigram analisis.
        /// </summary>
        /// <param name="guessWeight">
        /// The guess weight array.
        /// </param>
        private void UnigramAnalisis(double[,] guessWeight)
        {
            int x, y, curr, diff = engData.charFreq.Length - cipherTextData.uGramArray.Length;

            //Char freq weight
            for (int i = 0; i < cipherTextData.uGramArray.Length; i++)
            {
                for (int j = 0; j < wieghts.Length; j++)
                {
                    curr = i - 3 + j;
                    if (curr >= cipherTextData.uGramArray.Length)
                    {
                        curr = i;
                    }
                    if (curr > 0)
                    {
                        x = cipherTextData.uGramArray[curr].Item1[0] - 'A';
                        y = engData.charFreq[i + diff].Item1[0] - 'A';
                        guessWeight[x, y] += wieghts[j] * (50.0 / (101 - i));
                    }
                }
            }
        }

        /// <summary>  
        /// Modifies the guess weight array based on bigram analisis.
        /// </summary>
        /// <param name="guessWeight">
        /// The guess weight array.
        /// </param>
        private void BigramAnalisis(double[,] guessWeight)
        {
            int letter1, letter2;
            double countMod = 0, bigramMod;
            
            string bCipher;
            bool inArray1, inArray2;

            for (int i = 0; i < cipherTextData.bGramArray.Length; i++)
            {
                bCipher = cipherTextData.bGramArray[i].Item1;
                bigramMod = ((double) cipherTextData.bGramArray[i].Item2 / cipherTextData.bGramArray[cipherTextData.bGramArray.Length - 1].Item2) + .5;

                countMod = 0;

                letter1 = text.assign[cipherTextData.bGramArray[i].Item1[0] - 'A'][0];
                letter2 = text.assign[cipherTextData.bGramArray[i].Item1[1] - 'A'][0];

                inArray1 = letter1 == '-';
                inArray2 = letter2 == '-';

                if (inArray1 && inArray2 || !inArray1 && !inArray2)
                {
                    //Do nothing
                }
                else if (inArray1)
                {
                    for (int j = 0; j < 26; j++)
                    {
                        countMod += engData.bigramData[j, letter2 - 'A'];
                    }

                    countMod = countMod / 26;

                    for (int j = 0; j < 26; j++)
                    {
                        guessWeight[cipherTextData.bGramArray[i].Item1[0] - 'A', j] += ((engData.bigramData[j, letter2 - 'A'] / countMod * (bigramMod) - 1));
                    }
                }
                else
                {
                    for (int j = 0; j < 26; j++)
                    {
                        countMod += engData.bigramData[letter1 - 'A', j];
                    }

                    for (int j = 0; j < 26; j++)
                    {
                        guessWeight[cipherTextData.bGramArray[i].Item1[0] - 'A', j] += ((engData.bigramData[letter1 - 'A', j] / countMod * (bigramMod) - 1));
                    }
                }
            }
        }

        /// <summary>  
        /// Modifies the guess weight array based on trigram analisis.
        /// </summary>
        /// <param name="guessWeight">
        /// The guess weight array.
        /// </param>
        private void TrigramAnalisis(double[,] guessWeight)
        {
            int x, y;
            double mod;
            string tCipher, tEng;

            for (int i = 0; i < cipherTextData.tGramArray.Length; i++)
            {
                tCipher = cipherTextData.tGramArray[i].Item1;

                for (int j = 0; j < engData.trigramData.Length; j++)
                {
                    mod = GetTMod(i, j);

                    if (mod > 0)
                    {
                        tEng = engData.trigramData[j].Item1;

                        for (int k = 0; k < 3; k++)
                        {
                            x = tCipher[k] - 'A';
                            y = tEng[k] - 'A';

                            // change to guessWeight[x, y] += mod * (5.0 / (Math.Abs(i-j)+1));
                            guessWeight[x, y] += mod * (50.0 / (101 - j));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the trigram weights modifier based on how many character of the trigram has already shown up.
        /// </summary>
        /// <param name="pos1">
        /// The index of the ciphertext trigram in cipherTextData.tGramArray.
        /// </param>
        /// <param name="pos2">
        /// The index of the english trigram in engData.trigramData.
        /// </param>
        /// <returns>
        /// The weight modifier
        /// </returns>
        private double GetTMod(int pos1, int pos2)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (text.assign[cipherTextData.tGramArray[pos1].Item1[i] - 'A'] == engData.trigramData[pos2].Item1[i].ToString())
                {
                    count++;
                }
            }
            if (count == 0)
            {
                return 0;
            }
            else if (count == 1)
            {
                return .1;
            }
            else
            {
                return .5;
            }
        }

        //Sub Assignment

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the CharMap control.
        ///   </para>
        ///   <para>
        ///   Assigns ciphertext characters to their associated plaintext guess. 
        ///   Then print the decrypted plaintext.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void CharMap_Click(object sender, EventArgs e)
        {
            if (text != null)
            {
                text.setAssign(0, A.Text.ToUpper());
                text.setAssign(1, B.Text.ToUpper());
                text.setAssign(2, C.Text.ToUpper());
                text.setAssign(3, D.Text.ToUpper());
                text.setAssign(4, E.Text.ToUpper());
                text.setAssign(5, F.Text.ToUpper());
                text.setAssign(6, G.Text.ToUpper());
                text.setAssign(7, H.Text.ToUpper());
                text.setAssign(8, I.Text.ToUpper());
                text.setAssign(9, J.Text.ToUpper());
                text.setAssign(10, K.Text.ToUpper());
                text.setAssign(11, L.Text.ToUpper());
                text.setAssign(12, M.Text.ToUpper());
                text.setAssign(13, N.Text.ToUpper());
                text.setAssign(14, O.Text.ToUpper());
                text.setAssign(15, P.Text.ToUpper());
                text.setAssign(16, Q.Text.ToUpper());
                text.setAssign(17, R.Text.ToUpper());
                text.setAssign(18, S.Text.ToUpper());
                text.setAssign(19, T.Text.ToUpper());
                text.setAssign(20, U.Text.ToUpper());
                text.setAssign(21, V.Text.ToUpper());
                text.setAssign(22, W.Text.ToUpper());
                text.setAssign(23, X.Text.ToUpper());
                text.setAssign(24, Y.Text.ToUpper());
                text.setAssign(25, Z.Text.ToUpper());

                P2.Text = MakePrintable(text.Plaintext);
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }
        //End Sub

        //Affine

        /// <summary>
        ///   <para>
        ///   Handles the SelectedIndexChanged event of the AffCipherText1 control.
        ///   </para>
        ///   <para>
        ///   Makes a guess if the assignment text has changed.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void AffCipherText1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AffCipherText2.Text != "" && AffPlainText1.Text != "" && AffPlainText2.Text != "")
            {
                MakeAffGuess();
            }
        }

        /// <summary>
        ///   <para>
        ///   Handles the SelectedIndexChanged event of the AffCipherText2 control.
        ///   </para>
        ///   <para>
        ///   Makes a guess if the assignment text has changed.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void AffCipherText2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AffCipherText1.Text != "" && AffPlainText1.Text != "" && AffPlainText2.Text != "")
            {
                MakeAffGuess();
            }
        }


        /// <summary>
        ///   <para>
        ///   Handles the Click event of the AffGuess control.
        ///   </para>
        ///   <para>
        ///   Makes a guess based when the button is pressed.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void AffGuess_Click(object sender, EventArgs e)
        {
            if (AffCipherText1.Text != "" && AffCipherText1.Text != "" && AffPlainText1.Text != "" && AffPlainText2.Text != "")
            MakeAffGuess();
            else
                MessageBox.Show("No Guess Made", "Make a Guess", MessageBoxButtons.OK);
        }


        /// <summary>
        /// Uses the guess made to the find the encryption key and, if valid, 
        /// it uses that to decrypt the ciphertext. Then it diplays the plaintext.
        /// </summary>
        private void MakeAffGuess()
        {
            if (text != null)
            {
                Tuple<int, int> guess1;
                Tuple<int, int> guess2;

                guess1 = new Tuple<int, int>(AffCipherText1.Text.Substring(1, 1)[0] - 'A', AffPlainText1.Text.Substring(1, 1)[0] - 'A');
                guess2 = new Tuple<int, int>(AffCipherText2.Text.Substring(1, 1)[0] - 'A', AffPlainText2.Text.Substring(1, 1)[0] - 'A');

                int inverse = -1;

                inverse = GetZ26Inverse(guess1.Item2 - guess2.Item2);

                if (inverse > 0)
                {
                    text.a = MakePos(guess1.Item1 - guess2.Item1) * inverse % 26;
                    text.b = MakePos(guess1.Item1 - text.a * guess1.Item2) % 26;

                    int aInverse = GetZ26Inverse(text.a);
                    if (aInverse > 0)
                    {
                        for (int i = 0; i < 26; i++)
                        {
                            text.setAssign(i, ((char) (MakePos((i - text.b) * aInverse) % 26 + 'A')).ToString());
                        }

                        K3.Text = "A: " + text.a + " |B: " + text.b + "\n";
                        K3.Text += text.getAssign();
                        P3.Text = MakePrintable(text.Plaintext);
                    }
                    else
                    {
                        K3.Text = "Invalid Assignment";
                    }

                }
                else
                {
                    K3.Text = "Invalid Assignment";
                }
            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Gets the Z26 multiplicative inverse of an int.
        /// </summary>
        /// <param name="v">
        /// The int to invert.
        /// </param>
        /// <returns>
        /// v's multiplicative inverse in Z26.
        /// </returns>
        private int GetZ26Inverse(int v)
        {
            int vPos = MakePos(v);

            switch (vPos % 26)
            {
                case 1:
                    return 1;
                case 3:
                    return 9;
                case 5:
                    return 21;
                case 7:
                    return 15;
                case 9:
                    return 3;
                case 11:
                    return 19;
                case 15:
                    return 7;
                case 17:
                    return 23;
                case 19:
                    return 11;
                case 21:
                    return 5;
                case 23:
                    return 17;
                case 25:
                    return 25;
                default:
                    return -1;
            }
        }

        /// <summary>  
        /// Converts the number into its equivalent positive integer in Z26.
        /// </summary>
        /// <param name="v">
        /// The number to make positive.
        /// </param>
        /// <returns>
        /// v's positive equivalent.
        /// </returns>
        private int MakePos(int v)
        {
            int res = v;

            while (res < 0)
            {
                res += 26;
            }

            return res;
        }
        //End Affine

        //Hill Cipher

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the HillGuess control.
        ///   </para>
        ///   <para>Makes a guess if you click the button.</para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void HillGuess_Click(object sender, EventArgs e)
        {
            MakeHillGuess();
        }


        /// <summary>
        /// Makes a decryption matrix based on the guess, and uses it to 
        /// decrypt the ciphertext and then displays the plaintext.
        /// </summary>
        private void MakeHillGuess()
        {
            if (text != null)
            {
                int n = int.Parse(Hill_N.Text);
                int i;

                Matrix plain = null;
                Matrix cipher = null;
                Matrix cipherInverse = null;

                Tuple<string, string> guess1, guess2;

                guess1 = new Tuple<string, string>(HillCipherText1.Text.Substring(1, 2), HillPlainText1.Text.Substring(1, 2));
                guess2 = new Tuple<string, string>(HillCipherText2.Text.Substring(1, 2), HillPlainText2.Text.Substring(1, 2));


                int[] plainMatrix = new int[n * n];
                int[] cipherMatrix = new int[n * n];
                string p, c;

                c = guess1.Item1;
                p = guess1.Item2;
                for (i = 0; i < 2; i++)
                {
                    plainMatrix[i * n] = p[i] - 'A';
                    cipherMatrix[i * n] = c[i] - 'A';
                }

                c = guess2.Item1;
                p = guess2.Item2;
                for (i = 0; i < 2; i++)
                {
                    plainMatrix[i * n + 1] = p[i] - 'A';
                    cipherMatrix[i * n + 1] = c[i] - 'A';
                }

                plain = new Matrix(plainMatrix, n);
                cipher = new Matrix(cipherMatrix, n);

                cipherInverse = cipher.GetInverse();


                if (cipherInverse != null)
                {
                    text.keyInverse = Matrix.MultMatrix2(plain, cipherInverse);
                    text.key = text.keyInverse.GetInverse();

                    if (text.key != null)
                    {
                        text.KeyMatrixDecrypt();

                        K4.Text = "Encryption Key:\n" + text.key.ToString() + "\n\n";
                        K4.Text += "Decryption Key:\n" + text.keyInverse.ToString();
                        P4.Text = MakePrintable(text.Plaintext);
                    }
                    else
                    {
                        K4.Text = "Decryption Key Guess:\n" + text.keyInverse.ToString() + "\n\n";
                        K4.Text = "Invalid Key";
                    }
                }
                else
                {
                    K4.Text = "Invalid Guess";
                }

            }
            else
            {
                MessageBox.Show("No file selected", "Select a File", MessageBoxButtons.OK);
            }
            return;
        }

        /// <summary>
        ///   <para>
        ///   Handles the SelectedIndexChanged event of the HillCipherText1 control.
        ///   </para>
        ///   <para>
        ///   Make a guess if the text changes.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void HillCipherText1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillCipherText2.Text != "" && HillPlainText1.Text != "" && HillPlainText2.Text != "")
            {
                MakeHillGuess();
            }
        }

        /// <summary>
        ///   <para>
        ///   Handles the SelectedIndexChanged event of the HillCipherText2 control.
        ///   </para>
        ///   <para>
        ///   Make a guess if the text changes.
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void HillCipherText2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HillCipherText1.Text != "" && HillPlainText1.Text != "" && HillPlainText2.Text != "")
            {
                MakeHillGuess();
            }
        }
        //End Hill

        //Help Buttons

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the ShiftHelp control.
        ///   </para>
        ///   <para>
        ///   Displays info for the shift cipher and how to use the solver (ToDo).
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void ShiftHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Caesar_cipher");
        }

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the SubHelp control.
        ///   </para>
        ///   <para>
        ///   Displays info for the Substitution cipher and how to use the solver (ToDo).
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        /// 
        private void SubHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Substitution_cipher");
        }

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the AffineHelp control.
        ///   </para>
        ///   <para>
        ///   Displays info for the Affine cipher and how to use the solver (ToDo).
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void AffineHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Affine_cipher");
        }

        /// <summary>
        ///   <para>
        ///   Handles the Click event of the HillHelp control.
        ///   </para>
        ///   <para>
        ///   Displays info for the Hill cipher and how to use the solver (ToDo).
        ///   </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void HillHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Hill_cipher");
        }
        //End Help Buttons
    }
}
