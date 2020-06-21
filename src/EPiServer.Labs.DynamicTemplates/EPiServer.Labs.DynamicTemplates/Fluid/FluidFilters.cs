using Fluid;
using Fluid.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Fluid
{
	public static class FluidFilters
	{
		public static FluidValue Lower(FluidValue input, FilterArguments arguments, TemplateContext context)
		{
			return new StringValue(input.ToStringValue().ToLower());
		}

		public static FluidValue Upper(FluidValue input, FilterArguments arguments, TemplateContext context)
		{
			return new StringValue(input.ToStringValue().ToUpper());
		}

		public static FluidValue DaysAgo(FluidValue input, FilterArguments arguments, TemplateContext context)
		{
			if (input.Type != FluidValues.DateTime)
			{
				return input;
			}
			var daysAgo = DateTime.Now - DateTime.Parse(input.ToStringValue());
			return NumberValue.Create(daysAgo.Days);
		}

		public static FluidValue Format(FluidValue input, FilterArguments arguments, TemplateContext context)
		{
			if (input.Type == FluidValues.DateTime)
			{
				return new StringValue(DateTime.Parse(input.ToStringValue()).ToString(arguments.At(0).ToStringValue()));
			}

			if (input.Type == FluidValues.Number)
			{
				return new StringValue(double.Parse(input.ToStringValue()).ToString(arguments.At(0).ToStringValue()));
			}

			return new StringValue(input.ToStringValue());
		}
	}
}