using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mediator
{
    public partial class Form1 : Form
    {
        private Mediator mediator;

        public Form1()
        {
            InitializeComponent();
            InitializeMediator();
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
            button1.Click += button1_Click;
        }

        private void InitializeMediator()
        {
            mediator = new Mediator();
            mediator.RegisterColleague(new Dentist("Стоматолог"));
            mediator.RegisterColleague(new Gastroenterologist("Гастроентеролог"));
            mediator.RegisterColleague(new Traumatologist("Травматолог"));
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedItem = checkedListBox1.SelectedItem?.ToString();

            if (selectedItem != null)
            {
                string specialist = mediator.Request(selectedItem);
                if (specialist != null)
                {
                    label4.Text = specialist;
                    label3.Text = "Записуємо вас до необхідного спеціаліста";
                    label5.Text = "Запит прийнято";
                }
                else
                {
                    label3.Text = "Спеціаліст не знайдений";
                    label5.Text = "";
                }
            }
            else
            {
                label3.Text = "Виберіть запит";
                label5.Text = "";
            }
        }

    }

    public class Mediator
    {
        private Dictionary<string, Colleague> colleagues = new Dictionary<string, Colleague>();

        public void RegisterColleague(Colleague colleague)
        {
            colleagues[colleague.Name] = colleague;
        }

        public string Request(string request)
        {
            foreach (var colleague in colleagues.Values)
            {
                if (colleague.CanHandle(request))
                {
                    return colleague.Name;
                }
            }
            return null;
        }
    }

    public abstract class Colleague
    {
        protected Mediator mediator;

        public string Name { get; }

        public Colleague(string name)
        {
            Name = name;
        }

        public void SetMediator(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public abstract bool CanHandle(string request);
    }

    public class Dentist : Colleague
    {
        public Dentist(string name) : base(name) { }

        public override bool CanHandle(string request)
        {
            return request == "зуб";
        }
    }

    public class Gastroenterologist : Colleague
    {
        public Gastroenterologist(string name) : base(name) { }

        public override bool CanHandle(string request)
        {
            return request == "шлунок";
        }
    }

    public class Traumatologist : Colleague
    {
        public Traumatologist(string name) : base(name) { }

        public override bool CanHandle(string request)
        {
            return request == "спина";
        }
    }
}
