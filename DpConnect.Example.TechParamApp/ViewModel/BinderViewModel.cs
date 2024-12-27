﻿using DpConnect.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    //Класс, который будет осуществлять связь свойств воркера и сурсконфига
    //При его создании, мы передаем
    //тип воркера
    //Из этого типа будут получены свойства DpValue
    //Тип сурс конфигурации. Из этого типа будут получены свойства сурс конфигурации
    //Либо, мы можем передать соединение. Тогда биндер из соединения получит свойства сурс конфигурации
    //Так как биндить мы должны на уже созданное соединение

    //Это окей. По сути, мы конфигурируем воркер, а не тех параметр какой-то или еще чего
    //В дальнейшем, если нам нужно будет отображать состояние воркера в виде какого-то тех параметра,
    //то мы должны будем распознать этого воркера и назначить ему класс ВМ.
    //Мы не будем это делать при создании, так как загружая конфигурацию н ужно сделать то же самое


    // С другой стороны, проблема может быть еще и из за того, что я пытаюсь привязать воркера к созданным соединениям
    //Которые могут быть в свою очередь разных типов, поэтому "красиво" через дженерики эту задачу не решить
    //Мы определяем тип соединения в рантайме.
    public class CreateWorkerViewModelM<TWorker>
    {
        public CreateWorkerViewModelM(IDpConnection connection)
        {
            
        }
    }
}
