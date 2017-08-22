using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Farm.AppCommon
{
    public class BaseTable 
    {

        virtual protected void Validate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    OnInsertValidate();
                    break;
                case ChangeAction.Update:
                    OnUpdateValidate();
                    break;
                case ChangeAction.Delete:
                    OnDeleteValidate();
                    break;
                default:
                    break;
            }
        }

        virtual protected void OnInsertValidate()
        {
            return;
        }

        virtual protected void OnUpdateValidate()
        {
            return;
        }

        virtual protected void OnDeleteValidate()
        {
            return;
        }
    }
}
