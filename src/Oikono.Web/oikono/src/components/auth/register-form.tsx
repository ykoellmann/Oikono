import * as React from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useAuth } from "@/auth/AuthContext";
import { Link, useNavigate } from "react-router-dom";
import { FormLabel, FormMessage } from "@/components/ui/form";

const passwordRules = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

const RegisterSchema = z.object({
  firstName: z.string().min(2, "First name must be at least 2 characters").max(50, "First name must be at most 50 characters"),
  lastName: z.string().min(2, "Last name must be at least 2 characters").max(50, "Last name must be at most 50 characters"),
  email: z.string().min(1, "Email is required").email("Invalid email").max(254, "Email too long"),
  password: z
    .string()
    .min(8, "Password must be at least 8 characters")
    .regex(passwordRules, "Must include uppercase, lowercase, number and special character"),
});

type RegisterValues = z.infer<typeof RegisterSchema>;

export function RegisterForm({ className, ...props }: React.ComponentProps<"form">) {
  const { register } = useAuth();
  const [error, setError] = React.useState<string | null>(null);
  const navigate = useNavigate();

  const form = useForm<RegisterValues>({
    resolver: zodResolver(RegisterSchema),
    defaultValues: { firstName: "", lastName: "", email: "", password: "" },
    mode: "onTouched",
  });

  async function onSubmit(values: RegisterValues) {
    setError(null);
    try {
      await register(values);
      navigate("/", { replace: true });
    } catch (e) {
      const err = e as { response?: { data?: { title?: string } } };
      setError(err?.response?.data?.title || "Registration failed");
    }
  }

  function oauth(provider: "google" | "apple") {
    window.location.href = `/api/authentication/oauth/${provider}/challenge`;
  }

  return (
    <form className={cn("flex flex-col gap-6", className)} onSubmit={form.handleSubmit(onSubmit)} {...props}>
      <div className="flex flex-col gap-2">
        <h1 className="text-2xl font-bold">Create your account</h1>
        <p className="text-muted-foreground text-sm text-balance">
          Enter your information below to create your account
        </p>
      </div>
      <div className="grid gap-6">
        <div className="grid gap-3">
          <FormLabel className="text-left" htmlFor="firstName" invalid={!!form.formState.errors.firstName}>First name</FormLabel>
          <Input id="firstName" name="given-name" autoComplete="given-name" {...form.register("firstName")} />
          <FormMessage className="text-left">{form.formState.errors.firstName?.message as string}</FormMessage>
        </div>
        <div className="grid gap-3">
          <FormLabel className="text-left" htmlFor="lastName" invalid={!!form.formState.errors.lastName}>Last name</FormLabel>
          <Input id="lastName" name="family-name" autoComplete="family-name" {...form.register("lastName")} />
          <FormMessage className="text-left">{form.formState.errors.lastName?.message as string}</FormMessage>
        </div>
        <div className="grid gap-3">
          <FormLabel className="text-left" htmlFor="email" invalid={!!form.formState.errors.email}>Email</FormLabel>
          <Input id="email" name="email" type="email" placeholder="m@example.com" autoComplete="email" {...form.register("email")} />
          <FormMessage className="text-left">{form.formState.errors.email?.message as string}</FormMessage>
        </div>
        <div className="grid gap-3">
          <FormLabel className="text-left" htmlFor="password" invalid={!!form.formState.errors.password}>Password</FormLabel>
          <Input id="password" name="new-password" type="password" autoComplete="new-password" {...form.register("password")} />
          <FormMessage className="text-left">{form.formState.errors.password?.message as string}</FormMessage>
        </div>
        {error && <div className="text-red-600 text-sm">{error}</div>}
        <Button type="submit" className="w-full">
          Create account
        </Button>
        <div className="after:border-border relative text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t">
          <span className="bg-background text-muted-foreground relative z-10 px-2">
            Or continue with
          </span>
        </div>
        <div className="grid grid-cols-2 gap-2">
          <Button variant="outline" className="w-full" type="button" onClick={() => oauth("google")}>
            Google
          </Button>
          <Button variant="outline" className="w-full" type="button" onClick={() => oauth("apple")}>
            Apple
          </Button>
        </div>
      </div>
      <div className="text-center text-sm">
        Already have an account?{" "}
        <Link to="/login" className="underline underline-offset-4">
          Login
        </Link>
      </div>
    </form>
  );
}
