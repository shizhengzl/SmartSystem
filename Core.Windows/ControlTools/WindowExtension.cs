using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Repository;
using Core.UsuallyCommon;
using FreeSql;

namespace Core.Windows.ControlTools
{
    public class WindowExtension<T> : Form where T : class, new()
    {
        Type type = typeof(T);
        public T _objectself { get; set; }

        public bool IsInsert { get; set; }

        public List<Type> enumList = new List<Type>()
        {
            typeof(DataType), 
        };
        public WindowExtension(T objects, bool isInsert)
        {
            IsInsert = isInsert;
            _objectself = objects;
            var list = objects.GetPropertyList();

            var propretylist = objects.GetType().GetProperties();

            Int32 startIndexX = 20;
            Int32 startIndexY = 20;
            foreach (var proprety in propretylist)
            {
                if (proprety.PropertyType.Name == "List`1"
                    || (!proprety.PropertyType.Namespace.StartsWith("System") && !enumList.Any(x => x.Name == proprety.PropertyType.Name)))
                    continue;
                var val = proprety.GetValue(objects, null).ToStringExtension();
                Label label = new Label() { Name = $"lbl{proprety.Name}", Text = proprety.Name };

                dynamic textBox = new ComboBox();
                if (enumList.Any(x => x.Name == proprety.PropertyType.Name))
                {
                    textBox = new ComboBox() { Name = $"{proprety.Name}" };
                    var firstenum = enumList.FirstOrDefault(x => x.Name == proprety.PropertyType.Name);

                    
                    textBox.Items.AddRange(Core.UsuallyCommon.EnumExtensions.EnumToList(firstenum).ToArray());

                    //textBox.DataSource =   firstenum.GetListEnumClass();
                    //textBox.ValueMember = "Keys";
                    //textBox.DisplayMember = "Name"; 
                    textBox.SelectedText = val;
                }
                else if ("Boolean" == proprety.PropertyType.Name)
                {
                    textBox = new CheckBox() { Name = $"{proprety.Name}", Checked = val.ToBoolean() };
                }
                else
                {
                    if (proprety.Name.ToLower() == "outputpath" || proprety.Name.ToLower() == "appendcodeurl" || proprety.Name.ToLower() == "appendurl")
                    {
                        //textBox = new TreeComboBox() { Name = $"{proprety.Name}", Text = val, Width = 400 };
                        //textBox.ImageList = tools.imageList;
                    }
                    else
                        textBox = new TextBox() { Name = $"{proprety.Name}", Text = val, Width = 400, Multiline = true, Height = 60, ScrollBars = ScrollBars.Both };
                }

                if (proprety.Name.ToUpper() == "ID")
                    textBox.ReadOnly = true;

                label.Location = new Point(startIndexX, startIndexY);
                textBox.Location = new Point(label.Location.X + 100, label.Location.Y);

                this.Controls.Add(label);
                this.Controls.Add(textBox);
                startIndexY += 70;
            }

            Button btnSave = new Button() { Text = BtnEnum.Save.ToStringExtension() };
            Button btnCanel = new Button() { Text = BtnEnum.Canel.ToStringExtension() };

            btnSave.Click += BtnSave_Click;
            btnCanel.Click += BtnCanel_Click;
            btnSave.Location = new Point(startIndexX, startIndexY);
            btnCanel.Location = new Point(startIndexX + 80, startIndexY);

            this.Height = 750;
            this.AutoScroll = true;
            this.Width = 750;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = type.Name;

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCanel);
        }

        private void BtnCanel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetPropertyInfo(PropertyInfo property, dynamic value)
        {
            if (enumList.Any(x => x.Name == property.PropertyType.Name))
            { 
                property.SetValue(_objectself, value, null);
            }
            else
            {
                property.SetValue(_objectself, Convert.ChangeType(value, property.PropertyType), null);

            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

           
            // get value save
            foreach (var item in this.Controls)
            {
                dynamic tb = string.Empty;

                switch (item.GetType().Name)
                {
                    case "ComboBox":
                        tb = (item as ComboBox); 
                        SetPropertyInfo(_objectself.GetType().GetProperties().FirstOrDefault(x => x.Name == tb.Name), (int)Core.UsuallyCommon.EnumExtensions.ToEnum<DataType>(tb.Text));
                        break;
                    case "CheckBox":
                        tb = (item as CheckBox);
                        SetPropertyInfo(_objectself.GetType().GetProperties().FirstOrDefault(x => x.Name == tb.Name), tb.Checked);
                        break;
                    case "TextBox":
                        tb = (item as TextBox);
                        SetPropertyInfo(_objectself.GetType().GetProperties().FirstOrDefault(x => x.Name == tb.Name), tb.Text);
                        break;
                        //case "TreeComboBox":
                        //tb = (item as TreeComboBox);
                        //SetPropertyInfo(objectself.GetType().GetProperties().FirstOrDefault(x => x.Name == tb.Name), tb.Text);
                        //break;
                }
            }
            if (IsInsert)
                FreeSqlFactory._Freesql.Insert<T>(_objectself).ExecuteAffrows();

            else
            { 
                FreeSqlFactory._Freesql.Update<T>().SetSource(_objectself).ExecuteAffrows(); 
            }
               
            this.Close();
        }

    }
}
