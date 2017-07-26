using System;
using TeamGleason.Talk37.SpeechSupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TeamGleason.Talk37.Keyboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly TextToSpeechEngine _engine = new TextToSpeechEngine();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AddCharToMessage(string c)
        {
            result.Text += c;
        }

        private void RemoveCharFromMessage()
        {
            if (result.Text.Length > 0)
            {
                result.Text = result.Text.Remove((result.Text.Length - 1), 1);
            }
        }

        private void charButton_click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            String keyword = btn.Content.ToString();
            AddCharToMessage(keyword);
            if (shiftToggleButton.IsChecked == true)
            {
                shiftToggleButton.IsChecked = false;
                // Not implementing the case, where Shift is pressed with CAPS lock.
                allKeysToLower();
            }
        }

        private void ctrlButton_click(object sender, RoutedEventArgs e)
        {
            result.Text = "";
        }

        private void emojiButton_click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            AddCharToMessage(button.Content.ToString());
        }

        private void emoji2Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Wow!!, That's surprising...";
        }

        private void emoji3Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Urgh!!, I feel like throwing up...";
        }

        private void emoji4Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Grrrr!!, I am angry...";
        }

        private void emoji5Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Oh no!!, I am scared...";
        }

        private void emoji6Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Awww!!, I love you...";
        }

        private void emoji7Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Poop...";
        }

        private void emoji8Button_click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Wink Wink...";
        }

        private void spaceButton_click(object sender, RoutedEventArgs e)
        {
            AddCharToMessage(" ");
        }

        private void emoji0Button_Click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Yay!!, I am happy...";
        }

        private void wordBackButton_Click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "I am feeling sleepy...";
        }

        private void emoji9Button_Click(object sender, RoutedEventArgs e)
        {
            theSpeech.Text = "Okay";
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveCharFromMessage();
        }

        async void enterButton_Click(object sender, RoutedEventArgs e)
        {
            var text = result.Text;
            result.SelectAll();

            var stream = await _engine.SayText(text);

            TheMedia.SetSource(stream, stream.ContentType);
            TheMedia.Play();
        }

        private void allKeysToLower()
        {
            aButton.Content = (aButton.Content as string).ToLower();
            bButton.Content = (bButton.Content as string).ToLower();
            cButton.Content = (cButton.Content as string).ToLower();
            dButton.Content = (dButton.Content as string).ToLower();
            eButton.Content = (eButton.Content as string).ToLower();
            fButton.Content = (fButton.Content as string).ToLower();
            gButton.Content = (gButton.Content as string).ToLower();
            hButton.Content = (hButton.Content as string).ToLower();
            iButton.Content = (iButton.Content as string).ToLower();
            jButton.Content = (jButton.Content as string).ToLower();
            kButton.Content = (kButton.Content as string).ToLower();
            lButton.Content = (lButton.Content as string).ToLower();
            mButton.Content = (mButton.Content as string).ToLower();
            nButton.Content = (nButton.Content as string).ToLower();
            oButton.Content = (oButton.Content as string).ToLower();
            pButton.Content = (pButton.Content as string).ToLower();
            qButton.Content = (qButton.Content as string).ToLower();
            rButton.Content = (rButton.Content as string).ToLower();
            sButton.Content = (sButton.Content as string).ToLower();
            tButton.Content = (tButton.Content as string).ToLower();
            uButton.Content = (uButton.Content as string).ToLower();
            vButton.Content = (vButton.Content as string).ToLower();
            wButton.Content = (wButton.Content as string).ToLower();
            xButton.Content = (xButton.Content as string).ToLower();
            yButton.Content = (yButton.Content as string).ToLower();
            zButton.Content = (zButton.Content as string).ToLower();
        }

        private void allKeysToUpper()
        {
            aButton.Content = (aButton.Content as string).ToUpper();
            bButton.Content = (bButton.Content as string).ToUpper();
            cButton.Content = (cButton.Content as string).ToUpper();
            dButton.Content = (dButton.Content as string).ToUpper();
            eButton.Content = (eButton.Content as string).ToUpper();
            fButton.Content = (fButton.Content as string).ToUpper();
            gButton.Content = (gButton.Content as string).ToUpper();
            hButton.Content = (hButton.Content as string).ToUpper();
            iButton.Content = (iButton.Content as string).ToUpper();
            jButton.Content = (jButton.Content as string).ToUpper();
            kButton.Content = (kButton.Content as string).ToUpper();
            lButton.Content = (lButton.Content as string).ToUpper();
            mButton.Content = (mButton.Content as string).ToUpper();
            nButton.Content = (nButton.Content as string).ToUpper();
            oButton.Content = (oButton.Content as string).ToUpper();
            pButton.Content = (pButton.Content as string).ToUpper();
            qButton.Content = (qButton.Content as string).ToUpper();
            rButton.Content = (rButton.Content as string).ToUpper();
            sButton.Content = (sButton.Content as string).ToUpper();
            tButton.Content = (tButton.Content as string).ToUpper();
            uButton.Content = (uButton.Content as string).ToUpper();
            vButton.Content = (vButton.Content as string).ToUpper();
            wButton.Content = (wButton.Content as string).ToUpper();
            xButton.Content = (xButton.Content as string).ToUpper();
            yButton.Content = (yButton.Content as string).ToUpper();
            zButton.Content = (zButton.Content as string).ToUpper();
        }

        private void capsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (capsToggleButton.IsChecked == true)
            {
                allKeysToUpper();
            }
            else
            {
                allKeysToLower();
            }
        }

        private void shiftToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (shiftToggleButton.IsChecked == true)
            {
                allKeysToUpper();
            }
            else
            {
                allKeysToLower();
            }
        }
    }
}
