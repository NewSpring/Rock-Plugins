﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;
using Rock.Web.Cache;
using Rock.Workflow;

namespace Rock.Model
{
    /// <summary>
    /// Represents an <see cref="Rock.Model.WorkflowActionForm"/> (action or task) that is performed as part of a <see cref="Rock.Model.WorkflowActionForm"/>.
    /// </summary>
    [Table( "WorkflowActionForm" )]
    [DataContract]
    public partial class WorkflowActionForm : Model<WorkflowActionForm>
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the notification system email identifier.
        /// </summary>
        /// <value>
        /// The notification system email identifier.
        /// </value>
        [DataMember]
        public int? NotificationSystemEmailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include actions in notification].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include actions in notification]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeActionsInNotification { get; set; }
        
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the footer.
        /// </summary>
        /// <value>
        /// The footer.
        /// </value>
        [DataMember]
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets the delimited list of action buttons and actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        [MaxLength( 2000 )]
        [DataMember]
        public string Actions { get; set; }

        /// <summary>
        /// An optional text attribute that will be updated with the action that was selected
        /// </summary>
        /// <value>
        /// The action attribute unique identifier.
        /// </value>
        [DataMember]
        public Guid? ActionAttributeGuid { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the form attributes.
        /// </summary>
        /// <value>
        /// The form attributes.
        /// </value>
        [DataMember]
        public virtual ICollection<WorkflowActionFormAttribute> FormAttributes
        {
            get { return _formAttributes ?? ( _formAttributes = new Collection<WorkflowActionFormAttribute>() ); }
            set { _formAttributes = value; }
        }
        private ICollection<WorkflowActionFormAttribute> _formAttributes;

        /// <summary>
        /// Gets or sets the notification system email.
        /// </summary>
        /// <value>
        /// The notification system email.
        /// </value>
        public virtual SystemEmail NotificationSystemEmail {get;set;}

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowActionForm"/> class.
        /// </summary>
        public WorkflowActionForm()
        {
            IncludeActionsInNotification = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// To the liquid.
        /// </summary>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        /// <returns></returns>
        public override object ToLiquid( bool debug )
        {
            var mergeFields = base.ToLiquid( debug ) as Dictionary<string, object>;
            mergeFields.Add( "Buttons", GetButtonsLiquid() );
            return mergeFields;
        }

        private List<Dictionary<string, object>> GetButtonsLiquid()
        {
            var buttonList = new List<Dictionary<string, object>>();

            foreach ( var actionButton in Actions.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                var button = new Dictionary<string, object>();
                var details = actionButton.Split( new char[] { '^' } );
                if ( details.Length > 0 )
                {
                    button.Add( "Name", details[0] );

                    if ( details.Length > 1 )
                    {
                        var definedValue = DefinedValueCache.Read( details[1].AsGuid() );
                        if ( definedValue != null )
                        {
                            button.Add( "Html", definedValue.GetAttributeValue( "ButtonHTML" ) );
                        }
                    }
                }

                buttonList.Add( button );
            }

            return buttonList;
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Workflow Form Configuration class.
    /// </summary>
    public partial class WorkflowActionFormConfiguration : EntityTypeConfiguration<WorkflowActionForm>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowActionFormConfiguration"/> class.
        /// </summary>
        public WorkflowActionFormConfiguration()
        {
            this.HasOptional( f => f.NotificationSystemEmail ).WithMany().HasForeignKey( f => f.NotificationSystemEmailId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

}
