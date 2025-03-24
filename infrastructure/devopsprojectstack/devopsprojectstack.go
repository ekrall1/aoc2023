package devopsprojectstack

import (
	sdk "github.com/pulumi/pulumi-azure-native-sdk/v2"
	ad "github.com/pulumi/pulumi-azuread/sdk/v6/go/azuread"

	//devops "github.com/pulumi/pulumi-azuredevops/sdk/v3/go/azuredevops"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi/config"
)

type DevopsOutputs struct {
	//ProjectName      pulumi.StringOutput
	ProjectName              pulumi.StringOutput
	ServicePrincipalClientId pulumi.StringOutput
	ServicePrincipalTenant   pulumi.StringOutput
	ServicePrincipalSecret   pulumi.StringOutput
}

func DevopsStack(ctx *pulumi.Context, providerLocation *sdk.Provider) (DevopsOutputs, error) {

	devopsConfig := config.New(ctx, "devopsConfig")

	current, err := ad.GetClientConfig(ctx, pulumi.CompositeInvoke(), nil)
	if err != nil {
		return DevopsOutputs{}, err
	}

	aoc2023App, err := ad.NewApplication(ctx, devopsConfig.Require("appName"), &ad.ApplicationArgs{
		DisplayName: pulumi.String(devopsConfig.Require("appName")),
		Owners: pulumi.StringArray{
			pulumi.String(current.ObjectId),
		},
	})
	if err != nil {
		return DevopsOutputs{}, err
	}
	principal, err := ad.NewServicePrincipal(ctx, "service-principal", &ad.ServicePrincipalArgs{
		ClientId:                  aoc2023App.ClientId,
		AppRoleAssignmentRequired: pulumi.Bool(false),
		Owners: pulumi.StringArray{
			pulumi.String(current.ObjectId),
		},
	})
	if err != nil {
		return DevopsOutputs{}, err
	}
	password, err := ad.NewServicePrincipalPassword(ctx, "service-principal-pwd", &ad.ServicePrincipalPasswordArgs{
		ServicePrincipalId: principal.ID(),
	})
	if err != nil {
		return DevopsOutputs{}, err
	}

	// project, err := devops.NewProject(ctx, "aoc2023", &devops.ProjectArgs{
	// 	Name:             pulumi.String(devopsConfig.Require("projectName")),
	// 	Visibility:       pulumi.String("private"),
	// 	VersionControl:   pulumi.String("Git"),
	// 	WorkItemTemplate: pulumi.String("Agile"),
	// 	Features: pulumi.StringMap{
	// 		"testplans": pulumi.String("disabled"),
	// 		"artifacts": pulumi.String("enabled"),
	// 		"boards":    pulumi.String("enabled"),
	// 		"pipelines": pulumi.String("enabled"),
	// 	},
	// 	Description: pulumi.String("my devops project"),
	// }, pulumi.Provider(providerLocation))

	// if err != nil {
	// 	return DevopsOutputs{}, err
	// }

	return DevopsOutputs{
		ProjectName:              password.Value,
		ServicePrincipalClientId: principal.ID().ToStringOutput(),
		ServicePrincipalSecret:   password.Value,
		ServicePrincipalTenant:   principal.ApplicationTenantId,
	}, nil

}
