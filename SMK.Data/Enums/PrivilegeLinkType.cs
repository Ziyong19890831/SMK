using SMK.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMK.Data.Enums
{
    public enum PrivilegeNodeType
    {
        /// <summary>
        /// will never save to db, in program use
        /// </summary>
        Root,
        /// <summary>
        /// Just a menu node
        /// </summary>
        [Message("節點")]
        Node,

        [Message("連結節點")]
        Link,

        [Message("功能")]
        Functionality

    }
}