using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SystemInput
{
    private InputModel inputModel;
    public SystemInput(InputModel inputModel)
    {
        this.inputModel = inputModel;
    }
    public void Update()
    {
        inputModel.Horizontal = Input.GetAxis("Horizontal");
        inputModel.Vertical = Input.GetAxis("Vertical");
    }
}