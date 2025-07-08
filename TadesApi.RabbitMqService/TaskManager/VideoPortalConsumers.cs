using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using VideoPortalApi.Core;
using VideoPortalApi.RabbitMqService.Consumer;

namespace VideoPortalApi.RabbitMqService.TaskManager
{
    public static class VideoPortalConsumers
    {
        private static readonly int PrefetchCount = 4;
        private static readonly int ConcurrencyLimit = 4;
        public static void ConsumerExtensions(this IServiceCollection services, AppConfigs appsettings)
        {
            services.AddMassTransit(x =>
            {
                // add a specific consumer
                x.AddConsumer<ForgotPasswordConsumer>();
                x.AddConsumer<LoginConfirmConsumer>();
                x.AddConsumer<GeneratedUserEmailConsumer>();
                x.AddConsumer<ScheduleEventNotificationEmailConsumer>();
                x.AddConsumer<RoomInviteConsumer>();
                x.AddConsumer<InquiryReplyMessageConsumer>();
                x.AddConsumer<ScheduleEventConsultantNotificationEmailConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(appsettings.RabbitMqUri), hostConfigurator =>
                    {
                        hostConfigurator.Username(appsettings.RabbitMqUserName);
                        hostConfigurator.Password(appsettings.RabbitMqPassword);
                    });


                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._ForgotPasswordEmail, ec =>
                    {
                        ec.ConfigureConsumer<ForgotPasswordConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._LoginConfirmEmail, ec =>
                    {
                        ec.ConfigureConsumer<LoginConfirmConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._GeneratedCustomer, ec =>
                    {
                        ec.ConfigureConsumer<GeneratedUserEmailConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._ScheduleEventNotificationEmail, ec =>
                    {
                        ec.ConfigureConsumer<ScheduleEventNotificationEmailConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._RoomInviteEmail, ec =>
                    {
                        ec.ConfigureConsumer<RoomInviteConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._ContactReplyMessage, ec =>
                    {
                        ec.ConfigureConsumer<InquiryReplyMessageConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                    cfg.ReceiveEndpoint(Lsts.MassTransitJobName._ScheduleEventConsultantNotificationEmail, ec =>
                    {
                        ec.ConfigureConsumer<ScheduleEventConsultantNotificationEmailConsumer>(context);
                        ec.UseConcurrencyLimit(10000);
                        ec.UseMessageRetry(x => x.Immediate(5));
                    });

                });
            });
        }
    }
}
