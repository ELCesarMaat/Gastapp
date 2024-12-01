using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gastapp.Utils.Converters;

namespace Gastapp.Utils
{
    public class MonthTemplate
    {
        private DataTemplate circleTemplate;

        private DataTemplate template;

        public DataTemplate Template
        {
            get
            {
                return this.template;
            }
            set
            {
                this.template = value;
            }
        }

        public MonthTemplate()
        {
            this.circleTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid();

                Border border = new Border();
                border.BackgroundColor = Color.FromRgba("#F5F5F5");
                border.StrokeShape = new RoundRectangle()
                {
                    CornerRadius = new CornerRadius(25)
                };

                border.SetBinding(Border.StrokeThicknessProperty, "Date", converter: new DateToStrokeConverter());
                border.Stroke = Color.FromArgb("#0A3A74");

                Label label = new Label();
                label.SetBinding(Label.TextProperty, "Date.Day");
                label.HorizontalOptions = LayoutOptions.Center;
                label.VerticalOptions = LayoutOptions.Center;
                label.Padding = new Thickness(2);
                border.Content = label;

                grid.Add(border);
                grid.Padding = new Thickness(1);

                return grid;
            });

            this.template = this.circleTemplate;
        }
    }
}
